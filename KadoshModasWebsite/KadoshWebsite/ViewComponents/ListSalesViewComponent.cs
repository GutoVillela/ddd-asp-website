using KadoshWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.ViewComponents
{
    public class ListSalesViewComponent : ViewComponent
    {
        //public async Task<IViewComponentResult> InvokeAsync(int? filterByCustumerId)
        //{
        //    ListSalesComponentViewModel model = new()
        //    {
        //        FilterByCustomerId = filterByCustumerId
        //    };
        //    return View(model);
        //}

        public IViewComponentResult Invoke(int? filterByCustumerId)
        {
            ListSalesComponentViewModel model = new()
            {
                FilterByCustomerId = filterByCustumerId
            };
            return View(model);
        }
    }
}
