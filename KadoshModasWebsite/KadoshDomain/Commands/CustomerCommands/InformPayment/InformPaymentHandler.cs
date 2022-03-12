using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.CustomerCommands.InformPayment
{
    public class InformPaymentHandler : HandlerBase<InformPaymentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICustomerRepository _repository;
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;

        public InformPaymentHandler(IUnitOfWork unitOfWork, ICustomerRepository repository, ISaleRepository saleRepository, ICustomerPostingRepository customerPostingRepository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _saleRepository = saleRepository;
            _customerPostingRepository = customerPostingRepository;
        }

        public override async Task<ICommandResult> HandleAsync(InformPaymentCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_CUSTOMER_INFORM_PAYMENT_COMMAND);
                    return new CommandResult(false, CustomerCommandMessages.INVALID_CUSTOMER_INFORM_PAYMENT_COMMAND, errors);

                }

                // Validate customer
                Customer? customer = await _repository.ReadAsNoTrackingAsync(command.CustomerId!.Value);

                if (customer is null)
                {
                    AddNotification(nameof(customer), CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_CUSTOMER_NOT_FOUND);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_CUSTOMER_NOT_FOUND, errors);
                }

                // Get all opened sales
                var customersOpenSales = await _saleRepository.ReadAllOpenFromCustomerAsync(customer.Id);

                if (customersOpenSales is null || !customersOpenSales.Any())
                {
                    AddNotification(nameof(customersOpenSales), CustomerCommandMessages.ERROR_NO_OPEN_SALES_FOUND_TO_CUSTOMER);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_NO_OPEN_SALES_FOUND_TO_CUSTOMER);
                    return new CommandResult(false, CustomerCommandMessages.ERROR_NO_OPEN_SALES_FOUND_TO_CUSTOMER, errors);
                }

                decimal amountRemainingToInform = command.AmountToInform!.Value;

                foreach (var sale in customersOpenSales)
                {
                    if (amountRemainingToInform == 0)
                        break;

                    decimal valueToInform = 0;
                    bool payoffSale = false;

                    if (amountRemainingToInform >= sale.TotalToPay)
                    {
                        valueToInform = sale.TotalToPay;
                        amountRemainingToInform -= valueToInform;
                        payoffSale = true;
                    }
                    else
                    {
                        valueToInform = amountRemainingToInform;
                        amountRemainingToInform = 0;
                    }

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
                        var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_ONE_OR_MORE_INVALID_POSTINGS_TO_INFORMING_PAYMENT);
                        return new CommandResult(false, CustomerCommandMessages.ERROR_ONE_OR_MORE_INVALID_POSTINGS_TO_INFORMING_PAYMENT, errors);
                    }

                    await _customerPostingRepository.CreateAsync(customerPosting);

                    if (payoffSale)
                    {
                        sale.SetSituation(ESaleSituation.Completed);
                        sale.SetLastUpdateDate(DateTime.UtcNow);
                        await _saleRepository.UpdateAsync(sale);
                    }
                }

                // Commit changes
                await _unitOfWork.CommitAsync();

                if (amountRemainingToInform > 0)
                {
                    string valueInformed = (command.AmountToInform!.Value - amountRemainingToInform).ToString("C");
                    return new CommandResult(true, string.Format(CustomerCommandMessages.SUCCESS_ON_INFORM_PAYMENT_COMMAND_WITH_RESERVATIONS, command.AmountToInform!.Value.ToString("C"), valueInformed));
                }
                else
                    return new CommandResult(true, CustomerCommandMessages.SUCCESS_ON_INFORM_PAYMENT_COMMAND);

            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION + $" {ex.Message}", errors);
            }
        }
    }
}
