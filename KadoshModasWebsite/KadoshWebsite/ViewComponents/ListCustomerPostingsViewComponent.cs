using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.ViewComponents
{
    public class ListCustomerPostingsViewComponent : ViewComponent
    {
        private readonly ICustomerPostingApplicationService _customerPostingsService;

        public ListCustomerPostingsViewComponent(ICustomerPostingApplicationService customerPostingsService)
        {
            _customerPostingsService = customerPostingsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? filterByCustumerId)
        {
            IEnumerable<CustomerPostingViewModel> customerPosting = new List<CustomerPostingViewModel>();

            if(filterByCustumerId.HasValue)
                customerPosting = await _customerPostingsService.GetAllPostingsFromCustomerAsync(filterByCustumerId.Value);

            return View(customerPosting);
        }
    }
}
