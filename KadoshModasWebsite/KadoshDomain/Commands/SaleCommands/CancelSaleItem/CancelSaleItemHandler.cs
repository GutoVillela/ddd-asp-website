using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.SaleCommands.CancelSaleItem
{
    public class CancelSaleItemHandler : CommandHandlerBase<CancelSaleItemCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public CancelSaleItemHandler(IUnitOfWork unitOfWork, ISaleRepository saleRepository, ICustomerPostingRepository customerPostingRepository)
        {
            _unitOfWork = unitOfWork;
            _saleRepository = saleRepository;
            _customerPostingRepository = customerPostingRepository;
        }

        public override async Task<ICommandResult> HandleAsync(CancelSaleItemCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CANCEL_SALE_ITEM_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_CANCEL_SALE_ITEM_COMMAND, errors);

                }

                #region Validations
                // Validate sale
                Sale? sale = await _saleRepository.ReadAsync(command.SaleId!.Value);

                if (sale is null)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_SALE_NOT_FOUND);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE, errors);
                }

                // Sale must not be canceled
                if (sale.Situation == Enums.ESaleSituation.Canceled)
                {
                    AddNotification(nameof(sale.Situation), SaleCommandMessages.INVALID_SALE_SITUATION_ON_CANCEL_ITEM_COMMAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_SITUATION_ON_CANCEL_ITEM_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_SITUATION_ON_CANCEL_ITEM_COMMAND, errors);
                }

                // Check if the product it exists
                if (!sale.SaleItems.Any(x => x.ProductId == command.ProductId))
                {
                    AddNotification(nameof(sale.Situation), SaleCommandMessages.PRODUCT_NOT_FOUND_ON_CANCEL_ITEM_COMMAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_PRODUCT_NOT_FOUND_ON_CANCEL_ITEM_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.PRODUCT_NOT_FOUND_ON_CANCEL_ITEM_COMMAND, errors);
                }

                // Check if the product is already canceled
                if (sale.SaleItems.Where(x => x.ProductId == command.ProductId).All(x => x.Situation == Enums.ESaleItemSituation.Canceled))
                {
                    AddNotification(nameof(sale.Situation), SaleCommandMessages.INVALID_PRODUCT_SITUATION_ON_CANCEL_ITEM_COMMAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PRODUCT_SITUATION_ON_CANCEL_ITEM_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_PRODUCT_SITUATION_ON_CANCEL_ITEM_COMMAND, errors);
                }
                #endregion Validations

                var items = sale.SaleItems.Where(x => x.ProductId == command.ProductId && x.Situation != Enums.ESaleItemSituation.Canceled);

                // Check if there's enough items to cancel
                if (items.All(x => x.Amount < command.AmountToCancel))
                {
                    AddNotification(nameof(sale.Situation), SaleCommandMessages.INVALID_ITEMS_AMOUNT_ON_CANCEL_ITEM_COMMAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_ITEMS_AMOUNT_ON_CANCEL_ITEM_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_ITEMS_AMOUNT_ON_CANCEL_ITEM_COMMAND, errors);
                }

                decimal reversalPayment = 0;

                foreach (SaleItem item in items)
                {
                    if (item.Amount < command.AmountToCancel)
                        continue;

                    item.DecreaseAmount(command.AmountToCancel!.Value);
                    var newSaleItems = sale.SaleItems.ToList();

                    SaleItem canceledItem = item.Clone() as SaleItem;
                    canceledItem.SetAmount(command.AmountToCancel!.Value);

                    // If there's no canceled item them add item
                    if(item.Amount == 0)
                    {
                        newSaleItems.RemoveAll(x => x.ProductId == item.ProductId && x.Situation == item.Situation);
                    }
                    
                    if (!sale.SaleItems.Any(x => x.ProductId == command.ProductId && x.Situation == Enums.ESaleItemSituation.Canceled))
                    {
                        canceledItem.UpdateSituation(Enums.ESaleItemSituation.Canceled);
                        newSaleItems.Add(canceledItem);
                    }
                    else
                    {
                        var itemToIncrease = newSaleItems.FirstOrDefault(x => x.ProductId == command.ProductId && x.Situation == Enums.ESaleItemSituation.Canceled);
                        itemToIncrease.SetAmount(itemToIncrease.Amount + command.AmountToCancel!.Value);
                    }
                    

                    // Check if a reversal payment is needed
                    if(sale.TotalToPay < canceledItem.CalculateSaleItemTotal())
                    {
                        reversalPayment = canceledItem.CalculateSaleItemTotal() - sale.TotalToPay;

                        // Create customer posting
                        CustomerPosting customerPosting = new(
                            type: ECustomerPostingType.ReversalPayment,
                            value: reversalPayment,
                            sale: sale,
                            postingDate: DateTime.UtcNow
                            );

                        // Entity validations
                        AddNotifications(customerPosting);

                        // Check validations
                        if (!IsValid)
                        {
                            var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_CREATE_CASHBACK_POSTING);
                            return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_CREATE_CASHBACK_POSTING, errors);
                        }

                        // Register posting
                        await _customerPostingRepository.CreateAsync(customerPosting);
                    }

                    //Update sale item
                    sale.SetSaleItems(newSaleItems);

                    break;
                }

                // Check if sale must be canceled too
                if(sale.SaleItems.All(x => x.Situation == ESaleItemSituation.Canceled))
                    sale.SetSituation(ESaleSituation.Canceled);

                // Commit changes
                await _unitOfWork.CommitAsync();

                // Finish sale creation
                return new CommandResult(true, string.Format(SaleCommandMessages.SUCCESS_ON_CANCEL_SALE_ITEM_COMMAND, reversalPayment.ToString("N2")));
            }
            catch(Exception ex)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }

        }
    }
}
