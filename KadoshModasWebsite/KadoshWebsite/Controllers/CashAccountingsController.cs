using KadoshShared.ValueObjects;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.Util;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    public class CashAccountingsController : BaseController
    {
        private readonly IStoreApplicationService _storeService;

        public CashAccountingsController(IStoreApplicationService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            await SelectListLoaderHelper.LoadStoresToViewData(_storeService, ViewData);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(CashAccountingViewModel model)
        {
            if (ModelState.IsValid)
            {
                await SelectListLoaderHelper.LoadStoresToViewData(_storeService, ViewData);
            }

            return View(model);
        }

        protected override void AddErrorsToModelState(ICollection<Error> errors)
        {
            throw new NotImplementedException();
        }
    }
}
