using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.SaleCommands.InformPayment
{
    public class InformSalePaymentHandler : CommandHandlerBase<InformSalePaymentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public InformSalePaymentHandler(IUnitOfWork unitOfWork, ISaleRepository saleRepository, ICustomerPostingRepository customerPostingRepository)
        {
            _unitOfWork = unitOfWork;
            _saleRepository = saleRepository;
            _customerPostingRepository = customerPostingRepository;
        }

        public override async Task<ICommandResult> HandleAsync(InformSalePaymentCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_INFORM_SALE_PAYMENT_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_INFORM_PAYMENT_COMMAND, errors);

                }

                // Validate sale
                Sale? sale = await _saleRepository.ReadAsync(command.SaleId!.Value);

                if (sale is null)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_SALE_NOT_FOUND);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE, errors);
                }

                if(sale is SaleInInstallments)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.ERROR_CANNOT_INFORM_PAYMENT_TO_SALE_IN_INSTALLMENTS);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CANNOT_INFORM_PAYMENT_TO_SALE_IN_INSTALLMENTS);
                    return new CommandResult(false, SaleCommandMessages.ERROR_CANNOT_INFORM_PAYMENT_TO_SALE_IN_INSTALLMENTS, errors);
                }

                // Validate sale situation
                if(sale.Situation == ESaleSituation.Completed || sale.Situation == ESaleSituation.Canceled)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.INVALID_SITUATION_ON_INFORM_SALE_PAYMENT_COMMAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SITUATION_ON_INFORM_SALE_PAYMENT_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SITUATION_ON_INFORM_SALE_PAYMENT_COMMAND, errors);
                }

                // Validate amount to pay
                if(sale.TotalToPay <= 0)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.THERE_IS_NO_VALUE_TO_INFORM_ON_INFORM_SALE_PAYMENT_COMMAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_THERE_IS_NO_VALUE_TO_INFORM_ON_INFORM_SALE_PAYMENT_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.THERE_IS_NO_VALUE_TO_INFORM_ON_INFORM_SALE_PAYMENT_COMMAND, errors);
                }

                decimal valueToInform = command.AmountToInform!.Value;
                string successMessage = string.Empty;

                if (valueToInform >= sale.TotalToPay)
                {
                    valueToInform = sale.TotalToPay;

                    sale.SetSituation(ESaleSituation.Completed);
                    sale.SetLastUpdateDate(DateTime.UtcNow);
                    successMessage = string.Format(SaleCommandMessages.SUCCESS_ON_INFORM_SALE_PAYMENT_COMMAND_PAYOFF, valueToInform.ToString("C"));
                }
                else
                    successMessage = SaleCommandMessages.SUCCESS_ON_INFORM_SALE_PAYMENT_COMMAND;

                CustomerPosting customerPosting = new(
                        type: ECustomerPostingType.Payment,
                        value: valueToInform,
                        sale: sale,
                        postingDate: DateTime.UtcNow
                    );

                // Entity validations
                AddNotifications(customerPosting);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_POSTING_TO_INFORMING_PAYMENT);
                    return new CommandResult(false, SaleCommandMessages.ERROR_INVALID_POSTING_TO_INFORMING_PAYMENT, errors);
                }

                await _customerPostingRepository.CreateAsync(customerPosting);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, successMessage);

            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION + $" {ex.Message}", errors);
            }
        }
    }
}