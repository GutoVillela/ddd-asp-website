using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ValueObjects;
using KadoshWebsite.Infrastructure;
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
    public class ProductController : BaseController
    {
        private readonly IProductApplicationService _productService;
        private readonly IBrandApplicationService _brandService;
        private readonly ICategoryApplicationService _categoryService;

        public ProductController(IProductApplicationService service, IBrandApplicationService brandService, ICategoryApplicationService categoryService)
        {
            _productService = service;
            _brandService = brandService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int? page)
        {
            var products = await _productService.GetAllProductsPaginatedAsync(page ?? 1, PaginationManager.PAGE_SIZE);
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadBrandsAndCategoriesToViewData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductAsync(model);

                if (result.Success)
                {
                    ViewAlerts.SuccessAlert(TempData, result.Message);
                    return RedirectToAction(nameof(Index));
                }

                await LoadBrandsAndCategoriesToViewData();
                AddErrorsToModelState(errors: result.Errors);
                return View(model);
            }
            else
            {
                await LoadBrandsAndCategoriesToViewData();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var model = await _productService.GetProductAsync(id.Value);

            await LoadBrandsAndCategoriesToViewData(model.BrandId, model.CategoryId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProductAsync(model);

                if (result.Success)
                {
                    ViewAlerts.SuccessAlert(TempData, result.Message);
                    return RedirectToAction(nameof(Index));
                }

                await LoadBrandsAndCategoriesToViewData(model.BrandId, model.CategoryId);
                AddErrorsToModelState(errors: result.Errors);
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var model = await _productService.GetProductAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(UserViewModel model)
        {
            if (!model.Id.HasValue)
                return NotFound();

            var result = await _productService.DeleteProductAsync(model.Id.Value);

            if (result.Success)
            {
                ViewAlerts.SuccessAlert(TempData, result.Message);
                return RedirectToAction(nameof(Index));
            }

            AddErrorsToModelState(errors: result.Errors);
            return View(model);
        }

        private async Task LoadBrandsAndCategoriesToViewData(int? selectedBrand = null, int? selectedCategory = null)
        {
            await LoadBrandsToViewData(selectedBrand);
            await LoadCategoriesToViewData(selectedCategory);
        }

        private async Task LoadBrandsToViewData(int? selectedBrand = null)
        {
            var stores = await _brandService.GetAllBrandsAsync();
            ViewData[ViewConstants.VIEW_BRAND_SELECT_LIST_ITEMS] = GetSelectListItemsFromBrands(stores, selectedBrand);
        }

        private IEnumerable<SelectListItem> GetSelectListItemsFromBrands(IEnumerable<BrandViewModel> brands, int? selectedBrand = null)
        {
            List<SelectListItem> storeListItems = new();
            foreach (var brand in brands)
            {
                if (selectedBrand is not null && brand.Id == selectedBrand)
                    storeListItems.Add(new SelectListItem(text: brand.Name, value: brand.Id.ToString(),  selected: true));
                else
                    storeListItems.Add(new SelectListItem(text: brand.Name, value: brand.Id.ToString()));
            }
            return storeListItems;
        }

        private async Task LoadCategoriesToViewData(int? selectedCategory = null)
        {
            var stores = await _categoryService.GetAllCategoriesAsync();
            ViewData[ViewConstants.VIEW_CATEGORY_SELECT_LIST_ITEMS] = GetSelectListItemsFromCategories(stores, selectedCategory);
        }

        private IEnumerable<SelectListItem> GetSelectListItemsFromCategories(IEnumerable<CategoryViewModel> categories, int? selectedCategory = null)
        {
            List<SelectListItem> storeListItems = new();
            foreach (var category in categories)
            {
                if (selectedCategory is not null && category.Id == selectedCategory)
                    storeListItems.Add(new SelectListItem(text: category.Name, value: category.Id.ToString(), selected: true));
                else
                    storeListItems.Add(new SelectListItem(text: category.Name, value: category.Id.ToString()));
            }
            return storeListItems;
        }

        protected override void AddErrorsToModelState(ICollection<Error> errors)
        {
            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_PRODUCT_CREATE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_PRODUCT_CREATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_PRODUCT_UPDATE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_PRODUCT_UPDATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_PRODUCT_DELETE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_PRODUCT_DELETE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_PRODUCT_NOT_FOUND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_PRODUCT_NOT_FOUND));
        }
    }
}