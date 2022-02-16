using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.ViewComponents
{
    public class ListSalesViewComponent : ViewComponent
    {
        private readonly ISaleApplicationService _saleService;

        public ListSalesViewComponent(ISaleApplicationService saleService)
        {
            _saleService = saleService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int? filterByCustumerId)
        {
            IEnumerable<SaleViewModel> sales = new List<SaleViewModel>();

            if(filterByCustumerId.HasValue)
                sales = await _saleService.GetAllSalesByCustomerAsync(filterByCustumerId.Value);
            else
                sales = await _saleService.GetAllSalesAsync();

            return View(sales);
        }
    }
}
