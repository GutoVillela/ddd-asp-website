using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.ViewComponents
{
    public class ListCustomerPostingsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int? filterByCustumerId, int? filterBySaleId, DateTime? filterByDate, int? filterByStore, bool showTotal = false)
        {
            ListCustomerPostingsComponentViewModel model = new()
            {
                FilterByCustumerId = filterByCustumerId,
                FilterBySaleId = filterBySaleId,
                FilterByDate = filterByDate.HasValue ? DateOnly.FromDateTime(filterByDate.Value) : null,
                FilterByStore = filterByStore,
                ShowTotal = showTotal
            };

            return View(model);
        }
    }
}
