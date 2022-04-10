using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;

namespace KadoshDomain.Commands.SaleCommands.PayOffSale
{
    public class PayOffSaleHandler : CommandHandlerBase<PayOffSaleCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;
        private readonly IInstallmentRepository _installmentRepository;

        public PayOffSaleHandler(
            IUnitOfWork unitOfWork,
            ISaleRepository saleRepository,
            ICustomerPostingRepository customerPostingRepository,
            IInstallmentRepository installmentRepository)
        {
            _unitOfWork = unitOfWork;
            _saleRepository = saleRepository;
            _customerPostingRepository = customerPostingRepository;
            _installmentRepository = installmentRepository;
        }

        public override async Task<ICommandResult> HandleAsync(PayOffSaleCommand command)
        {
            try
            {
                // Fail Fast Validation
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PAYOFF_SALE_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_PAYOFF_SALE_COMMAND, errors);
                }

                // Validate sale
                var sale = await _saleRepository.ReadAsync(command.SaleId!.Value);

                if (sale is null)
                {
                    AddNotification(nameof(command.SaleId), SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_SALE_NOT_FOUND);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE, errors);
                }

                // PayOff all installments if sale is in installments
                if(sale is SaleInInstallments)
                {
                    var installmentsFromSale = await _installmentRepository.ReadAllInstallmentsFromSaleAsync(sale.Id);

                    foreach(Installment installment in installmentsFromSale)
                    {
                        if(installment.Situation == EInstallmentSituation.Open)
                        {
                            installment.PayOffInstallment();
                            await _installmentRepository.UpdateAsync(installment);
                        }
                    }
                }

                // Create customer posting
                CustomerPosting customerPosting = new(
                    type: ECustomerPostingType.Payment,
                    value: sale.TotalToPay,
                    sale: sale,
                    postingDate: DateTime.UtcNow
                    );

                // Entity validations
                AddNotifications(customerPosting);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_CREATE_SALE_PAYOFF_POSTING);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_CREATE_SALE_PAYOFF_POSTING, errors);
                }

                // Register posting
                await _customerPostingRepository.CreateAsync(customerPosting);

                // Change sale
                sale.SetSettlementDate(DateTime.UtcNow);
                sale.SetSituation(ESaleSituation.Completed);

                // Update sale
                await _saleRepository.UpdateAsync(sale);

                // Commit changes
                await _unitOfWork.CommitAsync();

                // Finish sale creation
                return new CommandResult(true, SaleCommandMessages.SUCCESS_ON_SALE_PAYOFF_COMMAND);
            }
            catch
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION, errors);
            }

        }
    }
}
