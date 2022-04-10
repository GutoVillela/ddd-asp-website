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

        [HttpPost]
        public async Task<IActionResult> PayOffSale(int? saleId)
        {
            if (saleId is null)
                return NotFound();

            var result = await _saleService.PayOffSaleAsync(saleId.Value);

            if (result.Success)
                return Ok();

            return BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<PartialViewResult> GetSalesPaginatedAsync(int? page, int? filterByCustumerId)
        {
            PaginatedListViewModel<SaleViewModel> sales;

            if(filterByCustumerId is null)
                sales = await _saleService.GetAllSalesPaginatedAsync(page ?? 1, PaginationManager.PAGE_SIZE);
            else
                sales = await _saleService.GetAllSalesByCustomerPaginatedAsync(filterByCustumerId!.Value, page ?? 1, PaginationManager.PAGE_SIZE);
            
            return PartialView("_SalesListTable", sales);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var model = await _saleService.GetSaleAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> InformPaymentAsync(int? saleId, decimal? amountToInform)
        {
            ArgumentNullException.ThrowIfNull(saleId);
            ArgumentNullException.ThrowIfNull(amountToInform);

            var result = await _saleService.InformPaymentAsync(saleId.Value, amountToInform.Value);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> PayOffInstallmentAsync(int? saleId, int? installmentId)
        {
            ArgumentNullException.ThrowIfNull(saleId);
            ArgumentNullException.ThrowIfNull(installmentId);

            var result = await _saleService.PayOffInstallmentAsync(saleId.Value, installmentId.Value);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
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
            if (errors.Any(x => x.Code == ErrorCodes.ERROR_COULD_NOT_FIND_SALE_CUSTOMER))
                ModelState.AddModelError(nameof(SaleViewModel.CustomerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_CUSTOMER));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_COULD_NOT_FIND_SALE_SELLER))
                ModelState.AddModelError(nameof(SaleViewModel.SellerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_SELLER));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_COULD_NOT_FIND_SALE_STORE))
                ModelState.AddModelError(nameof(SaleViewModel.StoreId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_STORE));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_SALE_IN_CASH_CREATE_COMMAND))
                ModelState.AddModelError(nameof(SaleViewModel.CustomerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_SALE_IN_CASH_CREATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_COULD_NOT_FIND_SALE_PRODUCT))
                ModelState.AddModelError(nameof(SaleViewModel.CustomerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_COULD_NOT_FIND_SALE_PRODUCT));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_SALE_ITEM))
                ModelState.AddModelError(nameof(SaleViewModel.CustomerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_SALE_ITEM));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_COULD_NOT_CREATE_SALE_IN_CASH_POSTING))
                ModelState.AddModelError(nameof(SaleViewModel.CustomerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_COULD_NOT_CREATE_SALE_IN_CASH_POSTING));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_SALE_IN_INSTALLMENTS_CREATE_COMMAND))
                ModelState.AddModelError(nameof(SaleViewModel.CustomerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_SALE_IN_INSTALLMENTS_CREATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_SALE_ON_CREDIT_CREATE_COMMAND))
                ModelState.AddModelError(nameof(SaleViewModel.CustomerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_SALE_ON_CREDIT_CREATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.UNEXPECTED_EXCEPTION))
                ModelState.AddModelError(nameof(SaleViewModel.CustomerId), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.UNEXPECTED_EXCEPTION));
        }
    }
}
