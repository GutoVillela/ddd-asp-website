using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.ViewComponents
{
    public class ProductSelectViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
