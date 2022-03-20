using KadoshWebsite.Infrastructure;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    public class CustomerPostingsController : Controller
    {

        private readonly ICustomerPostingApplicationService _customerPostingsService;

        public CustomerPostingsController(ICustomerPostingApplicationService customerPostingsService)
        {
            _customerPostingsService = customerPostingsService;
        }

        [HttpGet]
        public async Task<PartialViewResult> GetCustomerPostingsByCustomerPaginatedAsync(int? page, int? filterByCustumerId)
        {
            PaginatedListViewModel<CustomerPostingViewModel> customerPostings = new();

            if(filterByCustumerId.HasValue)
                customerPostings = await _customerPostingsService.GetAllPostingsFromCustomerPaginatedAsync(filterByCustumerId.Value, page ?? 1, PaginationManager.PAGE_SIZE);

            return PartialView("_CustomerPostingsListTable", customerPostings);
        }
    }
}
