using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.ViewComponents
{
    public class ListCustomerPostingsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int? filterByCustumerId)
        {
            ListCustomerPostingsComponentViewModel model = new()
            {
                FilterByCustumerId = filterByCustumerId
            };

            return View(model);
        }
    }
}
