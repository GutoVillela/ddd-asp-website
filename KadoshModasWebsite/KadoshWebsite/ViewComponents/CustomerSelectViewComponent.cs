
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.ViewComponents
{
    public class CustomerSelectViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

