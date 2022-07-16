using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ValueObjects;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Infrastructure.Authentication;
using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace KadoshWebsite.Controllers
{
    public class SaleController : BaseController
    {
        private readonly ICustomerApplicationService _customerService;
        private readonly IProductApplicationService _productService;
        private readonly ISaleApplicationService _saleService;
        private readonly IUserApplicationService _userService;
        private readonly IStoreApplicationService _storeService;
        private readonly ICustomerPostingApplicationService _customerPostingService;

        public SaleController(
            ICustomerApplicationService customerService, 
            IProductApplicationService productService,
            ISaleApplicationService saleService, 
            IUserApplicationService userService, 
            IStoreApplicationService storeService,
            ICustomerPostingApplicationService customerPostingService)
        {
            _customerService = customerService;
            _productService = productService;
            _saleService = saleService;
            _userService = userService;
            _storeService = storeService;
            _customerPostingService = customerPostingService;
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> IndexAsync()
        {
            var sales = await _saleService.GetAllSalesAsync();
            return View(sales);
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> CreateAsync()
        {
            await LoadCustomersSellersStoresAndProductsToViewData();
            return View(new SaleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
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
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> GetProductInfoAsync(int? productId)
        {
            if (productId is null)
                return NotFound();

            ProductViewModel product = await _productService.GetProductAsync(productId.Value);

            if (product is null)
                return NotFound();

            return Ok(product);
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> GetProductInfoByBarCodeAsync(string? barCode)
        {
            try
            {
                if (barCode is null)
                    return NotFound();

                ProductViewModel product = await _productService.GetProductByBarCodeAsync(barCode);

                if (product is null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
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
        [Authorize(Policy = nameof(LoggedInAuthorization))]
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
        [Authorize(Roles = Roles.CUSTOMER_ROLE)]
        public async Task<IActionResult> GetSalesByCustomerPaginatedAsync(int page, int pageSize, int custumerId)
        {
            try
            {
                if(page < 1 || pageSize < 1)
                    return BadRequest("O tamanho da página e a página devem ser maiores do que 0 para esta consulta");

                if (custumerId <= 0)
                    return BadRequest("O ID do cliente deve ser fornecido para esta consulta");

                PaginatedListViewModel<SaleViewModel> sales = await _saleService.GetAllSalesIncludingProductsByCustomerPaginatedAsync(custumerId, page, pageSize);

                return Ok(sales);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var model = await _saleService.GetSaleAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
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
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> CancelSaleAsync(int? saleId)
        {
            ArgumentNullException.ThrowIfNull(saleId);

            var result = await _saleService.CancelSaleAsync(saleId.Value);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpPost]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> PayOffInstallmentAsync(int? saleId, int? installmentId)
        {
            ArgumentNullException.ThrowIfNull(saleId);
            ArgumentNullException.ThrowIfNull(installmentId);

            var result = await _saleService.PayOffInstallmentAsync(saleId.Value, installmentId.Value);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> PrintReceiptAsync(int? saleId)
        {
            ArgumentNullException.ThrowIfNull(saleId);
            var model = new SaleReceiptViewModel
            {
                Sale = await _saleService.GetSaleAsync(saleId.Value)
            };

            var postings = await _customerPostingService.GetAllPostingsFromSalePaginatedAsync(model.Sale.Id!.Value, 0, 0);
            model.Postings = postings.Items;

            return new ViewAsPdf("PrintReceipt")
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = { Left = 1, Right = 1 },
                Model = model
            };
        }

        [HttpGet]
        [Authorize(Roles = Roles.CUSTOMER_ROLE)]
        public async Task<IActionResult> GetSaleByIdAsync(int saleId)
        {
            try
            {
                var sale = await _saleService.GetSaleAsync(saleId);

                if(sale is null)
                    return NotFound();

                return Ok(sale);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
