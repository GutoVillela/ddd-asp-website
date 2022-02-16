using KadoshShared.ValueObjects;
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
    public class SaleController : BaseController
    {
        private readonly ICustomerApplicationService _customerService;
        private readonly IProductApplicationService _productService;
        private readonly ISaleApplicationService _saleService;
        private readonly IUserApplicationService _userService;
        private readonly IStoreApplicationService _storeService;

        public SaleController(ICustomerApplicationService customerService, IProductApplicationService productService, ISaleApplicationService saleService, IUserApplicationService userService, IStoreApplicationService storeService)
        {
            _customerService = customerService;
            _productService = productService;
            _saleService = saleService;
            _userService = userService;
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var sales = await _saleService.GetAllSalesAsync();
            return View(sales);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCustomersSellersStoresAndProductsToViewData();
            return View(new SaleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(SaleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _saleService.CreateSaleAsync(model);

                if (result.Success)
                {
                    ViewAlerts.SuccessAlert(TempData, result.Message);
                    return RedirectToAction(nameof(Index));
                }

                AddErrorsToModelState(errors: result.Errors);

                await LoadCustomersSellersStoresAndProductsToViewData();
                return View(model);
            }
            else
            {
                await LoadCustomersSellersStoresAndProductsToViewData();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProductInfo(int? productId)
        {
            if (productId is null)
                return NotFound();

            ProductViewModel product = await _productService.GetProductAsync(productId.Value);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        private async Task LoadCustomersSellersStoresAndProductsToViewData()
        {
            await SelectListLoaderHelper.LoadCustomersToViewData(_customerService, ViewData);
            await SelectListLoaderHelper.LoadSellersToViewData(_userService, ViewData);
            await SelectListLoaderHelper.LoadStoresToViewData(_storeService, ViewData);
            await SelectListLoaderHelper.LoadProductsToViewData(_productService, ViewData);
        }

        protected override void AddErrorsToModelState(ICollection<Error> errors)
        {
            throw new NotImplementedException();
        }
    }
}
