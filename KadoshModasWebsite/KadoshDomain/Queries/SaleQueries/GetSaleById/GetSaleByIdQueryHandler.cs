using KadoshDomain.Entities;
using KadoshDomain.Queries.Base;
using KadoshDomain.Queries.SaleQueries.DTOs;
using KadoshDomain.Repositories;
using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.QueriesMessages;

namespace KadoshDomain.Queries.SaleQueries.GetSaleById
{
    public class GetSaleByIdQueryHandler : QueryHandlerBase<GetSaleByIdQuery, GetSaleByIdQueryResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IInstallmentRepository _installmentRepository;

        public GetSaleByIdQueryHandler(ISaleRepository saleRepository, IInstallmentRepository installmentRepository)
        {
            _saleRepository = saleRepository;
            _installmentRepository = installmentRepository;
        }

        public override async Task<GetSaleByIdQueryResult> HandleAsync(GetSaleByIdQuery query)
        {
            // Fail Fast Validations
            query.Validate();
            if (!query.IsValid)
            {
                AddNotifications(query);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_INVALID_GET_SALE_BY_ID_QUERY);
                return new GetSaleByIdQueryResult(errors);
            }

            var sale = await _saleRepository.ReadAsync(query.SaleId!.Value);

            if (sale is null)
            {
                AddNotification(nameof(sale), SaleQueriesMessages.ERROR_SALE_NOT_FOUND);
                var errors = GetErrorsFromNotifications(ErrorCodes.ERROR_SALE_NOT_FOUND);
                return new GetSaleByIdQueryResult(errors);
            }

            //Create DTO
            SaleBaseDTO saleDTO = sale;

            // Query installments if sale is in Installments
            if (sale is SaleInInstallments)
            {
                var installments = await _installmentRepository.ReadAllInstallmentsFromSaleAsync(query.SaleId!.Value);

                List<SaleInstallmentDTO> installmentsDTO = new();

                foreach (var installment in installments)
                {
                    installmentsDTO.Add(installment);
                }

                (saleDTO as SaleInInstallmentsDTO).Installments = installmentsDTO;
            }

            GetSaleByIdQueryResult result = new()
            {
                Sale = saleDTO
            };

            return result;
        }
    }
}
