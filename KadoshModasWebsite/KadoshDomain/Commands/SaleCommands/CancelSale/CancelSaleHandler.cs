using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.SaleCommands.CancelSale
{
    public class CancelSaleHandler : CommandHandlerBase<CancelSaleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public CancelSaleHandler(IUnitOfWork unitOfWork, ISaleRepository saleRepository, ICustomerPostingRepository customerPostingRepository)
        {
            _unitOfWork = unitOfWork;
            _saleRepository = saleRepository;
            _customerPostingRepository = customerPostingRepository;
        }

        public override async Task<ICommandResult> HandleAsync(CancelSaleCommand command)
        {
            // Fail Fast Validations
            command.Validate();
            if (!command.IsValid)
            {
                AddNotifications(command);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CANCEL_SALE_COMMAND);
                return new CommandResult(false, SaleCommandMessages.INVALID_CANCEL_SALE_COMMAND, errors);

            }

            // Validate sale
            Sale? sale = await _saleRepository.ReadAsync(command.SaleId!.Value);

            if (sale is null)
            {
                AddNotification(nameof(sale), SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_SALE_NOT_FOUND);
                return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE, errors);
            }

            if(sale.TotalPaid > 0)
            {
                // Create customer posting
                CustomerPosting customerPosting = new(
                    type: ECustomerPostingType.ReversalPayment,
                    value: sale.TotalPaid,
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

            // Update sale
            sale.SetSituation(ESaleSituation.Canceled);

            // Cancel items
            List<SaleItem> newItems = new();
            foreach(var item in sale.SaleItems)
            {
                var newItem = item.Clone() as SaleItem;
                newItem.UpdateSituation(ESaleItemSituation.Canceled);
                newItems.Add(newItem);
            }

            sale.SetSaleItems(newItems);

            // Commit changes
            await _unitOfWork.CommitAsync();

            // Finish sale creation
            return new CommandResult(true, SaleCommandMessages.SUCCESS_ON_CANCEL_SALE_COMMAND);

        }
    }
}
