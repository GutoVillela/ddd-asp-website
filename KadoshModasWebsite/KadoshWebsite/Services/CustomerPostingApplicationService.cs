using KadoshDomain.Queries.CustomerPostingQueries.DTOs;
using KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromCustomer;
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

        public CustomerPostingApplicationService(
            IQueryHandler<GetAllPostingsFromCustomerQuery, GetAllPostingsFromCustomerQueryResult> getAllPostingsFromCustomerQueryHandler,
            IQueryHandler<GetAllPostingsFromSaleQuery, GetAllPostingsFromSaleQueryResult> getAllPostingsFromSaleQueryHandler)
        {
            _getAllPostingsFromCustomerQueryHandler = getAllPostingsFromCustomerQueryHandler;
            _getAllPostingsFromSaleQueryHandler = getAllPostingsFromSaleQueryHandler;
        }

        public async Task<PaginatedListViewModel<CustomerPostingViewModel>> GetAllPostingsFromCustomerPaginatedAsync(int customerId, int currentPage, int pageSize)
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

            PaginatedListViewModel<CustomerPostingViewModel> paginatedList = new();
            paginatedList.CurrentPage = currentPage;
            paginatedList.PageSize = pageSize;
            paginatedList.TotalItemsCount = result.CustomerPostingsCount;
            paginatedList.TotalPages = PaginationManager.CalculateTotalPages(result.CustomerPostingsCount, pageSize);
            paginatedList.Items = customerPostingsViewModel;

            return paginatedList;

        }

        public async Task<PaginatedListViewModel<CustomerPostingViewModel>> GetAllPostingsFromSalePaginatedAsync(int saleId, int currentPage, int pageSize)
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

            PaginatedListViewModel<CustomerPostingViewModel> paginatedList = new();
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
