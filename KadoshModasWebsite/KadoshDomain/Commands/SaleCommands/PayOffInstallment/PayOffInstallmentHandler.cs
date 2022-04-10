using KadoshDomain.Commands.Base;
using KadoshDomain.Entities;
using KadoshDomain.Enums;
using KadoshDomain.Queriables;
using KadoshDomain.Repositories;
using KadoshShared.Commands;
using KadoshShared.Constants.CommandMessages;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Repositories;
using System.Linq;

namespace KadoshDomain.Commands.SaleCommands.PayOffInstallment
{
    public class PayOffInstallmentHandler : CommandHandlerBase<PayOffInstallmentCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerPostingRepository _customerPostingRepository;
        private readonly IInstallmentRepository _installmentRepository;

        public PayOffInstallmentHandler(
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

        public override async Task<ICommandResult> HandleAsync(PayOffInstallmentCommand command)
        {
            try
            {
                // Fail Fast Validations
                command.Validate();
                if (!command.IsValid)
                {
                    AddNotifications(command);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PAY_OFF_INSTALLMENT_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_PAYOFF_INSTALLMENT_COMMAND, errors);

                }

                // Validate sale
                Sale? sale = await _saleRepository.ReadAsync(command.SaleId!.Value);

                if (sale is null)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_SALE_NOT_FOUND);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE, errors);
                }

                if (sale is not SaleInInstallments)
                {
                    AddNotification(nameof(sale), string.Format(SaleCommandMessages.INVALID_PAYOFF_INSTALLMENT_SALE, command.SaleId));
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_PAYOFF_INSTALLMENT_SALE);
                    return new CommandResult(false, SaleCommandMessages.INVALID_PAYOFF_INSTALLMENT_SALE, errors);
                }

                // Validate sale situation
                if (sale.Situation == ESaleSituation.Completed || sale.Situation == ESaleSituation.Canceled)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.INVALID_SALE_SITUATION_ON_PAYOFF_INSTALLMENT_COMMAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_SALE_SITUATION_ON_PAYOFF_INSTALLMENT_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_SALE_SITUATION_ON_PAYOFF_INSTALLMENT_COMMAND, errors);
                }

                // Validate Installment
                Installment? installment = await _installmentRepository.ReadAsync(command.InstallmentId!.Value);

                if(installment is null)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_INSTALLMENT);
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_COULD_NOT_FIND_SALE_INSTALLMENT);
                    return new CommandResult(false, SaleCommandMessages.ERROR_COULD_NOT_FIND_SALE_INSTALLMENT, errors);
                }

                if(installment.Situation != EInstallmentSituation.Open)
                {
                    AddNotification(nameof(sale), SaleCommandMessages.INVALID_INSTALLMENT_SITUATION_ON_PAYOFF_INSTALLMENT_COMMAND);
                    var errors = GetErrorsFromNotifications(ErrorCodes.INVALID_INSTALLMENT_SITUATION_ON_PAYOFF_INSTALLMENT_COMMAND);
                    return new CommandResult(false, SaleCommandMessages.INVALID_INSTALLMENT_SITUATION_ON_PAYOFF_INSTALLMENT_COMMAND, errors);
                }

                installment.PayOffInstallment();
                installment.SetLastUpdateDate(DateTime.UtcNow);

                // Update Installment
                await _installmentRepository.UpdateAsync(installment);

                // Check if that was the last installment to PayOff
                var installmentsFromSale = await _installmentRepository.ReadAllInstallmentsFromSaleAsync(sale.Id);
                if (!installmentsFromSale.Any(InstallmentQueriable.GetBySituationAndSaleIdExceptFromOne(EInstallmentSituation.Open, sale.Id, installment.Id)))
                {
                    sale.SetLastUpdateDate(DateTime.UtcNow);
                    sale.SetSituation(ESaleSituation.Completed);
                    await _saleRepository.UpdateAsync(sale);
                }

                CustomerPosting customerPosting = new(
                        type: ECustomerPostingType.Payment,
                        value: installment.Value,
                        sale: sale,
                        postingDate: DateTime.UtcNow
                    );

                // Entity validations
                AddNotifications(customerPosting);

                // Check validations
                if (!IsValid)
                {
                    var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_POSTING_TO_PAYOFF_INSTALLMENT);
                    return new CommandResult(false, SaleCommandMessages.ERROR_INVALID_POSTING_TO_PAYOFF_INSTALLMENT, errors);
                }

                await _customerPostingRepository.CreateAsync(customerPosting);

                // Commit changes
                await _unitOfWork.CommitAsync();

                return new CommandResult(true, SaleCommandMessages.SUCCESS_ON_PAYOFF_INSTALLMENT_COMMAND);

            }
            catch (Exception ex)
            {
                var errors = GetErrorsFromNotifications(ErrorCodes.UNEXPECTED_EXCEPTION);
                return new CommandResult(false, SaleCommandMessages.UNEXPECTED_EXCEPTION + $" {ex.Message}", errors);
            }
        }
    }
}
