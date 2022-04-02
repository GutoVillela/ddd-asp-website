using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromCustomer;
using KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromDate;
using KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromSale;
using KadoshShared.ExtensionMethods;
using KadoshShared.Handlers;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;

namespace KadoshWebsite.Services
{
    public class CustomerPostingApplicationService : ICustomerPostingApplicationService
    {
        private readonly IQueryHandler<GetAllPostingsFromCustomerQuery, GetAllPostingsFromCustomerQueryResult> _getAllPostingsFromCustomerQueryHandler;
        private readonly IQueryHandler<GetAllPostingsFromSaleQuery, GetAllPostingsFromSaleQueryResult> _getAllPostingsFromSaleQueryHandler;
        private readonly IQueryHandler<GetAllPostingsFromStoreAndDateQuery, GetAllPostingsFromStoreAndDateQueryResult> _getAllPostingsFromDateQueryHandler;

        public CustomerPostingApplicationService(
            IQueryHandler<GetAllPostingsFromCustomerQuery, GetAllPostingsFromCustomerQueryResult> getAllPostingsFromCustomerQueryHandler,
            IQueryHandler<GetAllPostingsFromSaleQuery, GetAllPostingsFromSaleQueryResult> getAllPostingsFromSaleQueryHandler,
            IQueryHandler<GetAllPostingsFromStoreAndDateQuery, GetAllPostingsFromStoreAndDateQueryResult> getAllPostingsFromDateQueryHandler)
        {
            _getAllPostingsFromCustomerQueryHandler = getAllPostingsFromCustomerQueryHandler;
            _getAllPostingsFromSaleQueryHandler = getAllPostingsFromSaleQueryHandler;
            _getAllPostingsFromDateQueryHandler = getAllPostingsFromDateQueryHandler;
        }

        public async Task<PaginatedCustomerPostingsViewModel> GetAllPostingsFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize)
        {
            GetAllPostingsFromCustomerQuery query = new();
            query.CustomerId = customerId;
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllPostingsFromCustomerQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<CustomerPostingViewModel> customerPostingsViewModel = new();

            foreach (var posting in result.CustomerPostings)
            {
                customerPostingsViewModel.Add(GetViewModelFromDTO(posting));
            }

            PaginatedCustomerPostingsViewModel paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.CustomerPostingsCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.CustomerPostingsCount, pageSize);
            paginatedList.Items = customerPostingsViewModel;

            return paginatedList;

        }

        public async Task<PaginatedCustomerPostingsViewModel> GetAllPostingsFromStoreAndDatePaginatedAsync(DateOnly date, TimeZoneInfo localTimeZone, int storeId, bool getTotal, int currentPage, int pageSize)
        {
            GetAllPostingsFromStoreAndDateQuery query = new();
            query.LocalDate = date;
            query.LocalTimeZone = localTimeZone;
            query.StoreId = storeId;
            query.GetTotal = getTotal;
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllPostingsFromDateQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<CustomerPostingViewModel> customerPostingsViewModel = new();

            foreach (var posting in result.CustomerPostings)
            {
                customerPostingsViewModel.Add(GetViewModelFromDTO(posting));
            }

            PaginatedCustomerPostingsViewModel paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.CustomerPostingsCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.CustomerPostingsCount, pageSize);
            paginatedList.Items = customerPostingsViewModel;
            paginatedList.TotalCredit = result.TotalCredit;
            paginatedList.TotalDebit = result.TotalDebit;
            paginatedList.ShowTotal = getTotal;

            return paginatedList;
        }

        public async Task<PaginatedCustomerPostingsViewModel> GetAllPostingsFromSalePaginatedAsync(int saleId, int currentPage, int pageSize)
        {
            GetAllPostingsFromSaleQuery query = new();
            query.SaleId = saleId;
            query.CurrentPage = currentPage;
            query.PageSize = pageSize;

            var result = await _getAllPostingsFromSaleQueryHandler.HandleAsync(query);

            if (!result.Success)
                throw new ApplicationException(result.Errors!.GetAsSingleMessage());

            List<CustomerPostingViewModel> customerPostingsViewModel = new();

            foreach (var posting in result.CustomerPostings)
            {
                customerPostingsViewModel.Add(GetViewModelFromDTO(posting));
            }

            PaginatedCustomerPostingsViewModel paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.CustomerPostingsCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.CustomerPostingsCount, pageSize);
            paginatedList.Items = customerPostingsViewModel;

            return paginatedList;
        }

        private CustomerPostingViewModel GetViewModelFromDTO(CustomerPostingDTO customerPostingDTO)
        {
            return new CustomerPostingViewModel
            {
                SaleId = customerPostingDTO.SaleId,
                PostingDate = customerPostingDTO.PostingDate,
                Value = customerPostingDTO.Value,
                PostingType = customerPostingDTO.PostingType
            };
        }
    }
}
