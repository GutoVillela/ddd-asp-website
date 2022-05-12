using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KadoshWebsite.Controllers
{
    [Authorize(Policy = nameof(LoggedInAuthorization))]
    public class ImportFromLegacyController : Controller
    {
        private readonly IBrandApplicationService _brandService;
        private readonly ICategoryApplicationService _categoryService;
        private readonly IStoreApplicationService _storeService;
        private readonly IUserApplicationService _userService;
        private readonly ISettingsApplicationService _settingsService;

        public ImportFromLegacyController(IBrandApplicationService brandService, ICategoryApplicationService categoryService, IStoreApplicationService storeService, ISettingsApplicationService settingsService, IUserApplicationService userService)
        {
            _brandService = brandService;
            _categoryService = categoryService;
            _storeService = storeService;
            _settingsService = settingsService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            await LoadStoresBrandsAndCategoriesToViewDataAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(ImportFromLegacyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _settingsService.ImportDataFromLegacyAsync(model);

                if (result.Success)
                {
                    ViewAlerts.SuccessAlert(TempData, result.Message);
                    return RedirectToAction(nameof(IndexAsync).Replace("Async", ""));
                }
                else
                    ViewAlerts.ErrorAlert(TempData, result.Message);
            }
            
            await LoadStoresBrandsAndCategoriesToViewDataAsync(
                selectedStore: model.DefaultStoreId,
                selectedSeller: model.DefaultSellerId,
                selectedBrand: model.DefaultBrandId,
                selectedCategory: model.DefaultCategoryId
                );
            return View(model);
            
        }

        private async Task LoadStoresBrandsAndCategoriesToViewDataAsync(int? selectedStore = null, int? selectedSeller = null, int? selectedBrand = null, int? selectedCategory = null)
        {
            await SelectListLoaderHelper.LoadStoresToViewData(_storeService, ViewData, selectedStore);
            await SelectListLoaderHelper.LoadSellersToViewData(_userService, ViewData, selectedStore);
            await SelectListLoaderHelper.LoadBrandsToViewDataAsync(_brandService, ViewData, selectedBrand);
            await SelectListLoaderHelper.LoadCategoriesToViewDataAsync(_categoryService, ViewData, selectedCategory);
        }
    }
}
