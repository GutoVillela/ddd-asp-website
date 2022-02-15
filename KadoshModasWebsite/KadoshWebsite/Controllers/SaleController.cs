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

        public SaleController(ICustomerApplicationService customerService, IProductApplicationService productService, ISaleApplicationService saleService, IUserApplicationService userService)
        {
            _customerService = customerService;
            _productService = productService;
            _saleService = saleService;
            _userService = userService;
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
            await LoadCustomersSellersAndProductsToViewData();
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

                await LoadCustomersSellersAndProductsToViewData();
                return View(model);
            }
            else
            {
                await LoadCustomersSellersAndProductsToViewData();
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

        private async Task LoadCustomersSellersAndProductsToViewData()
        {
            await LoadCustomersToViewData();
            await LoadSellersToViewData();
            await LoadProductsToViewData();
        }

        private async Task LoadCustomersToViewData(int? selectedCustomer = null)
        {
            var customers = await _customerService.GetAllCustomersAsync();
            ViewData[ViewConstants.VIEW_CUSTOMERS_SELECT_LIST_ITEMS] = GetSelectListItemsFromCustomers(customers, selectedCustomer);
        }

        private IEnumerable<SelectListItem> GetSelectListItemsFromCustomers(IEnumerable<CustomerViewModel> customers, int? selectedCustomer = null)
        {
            List<SelectListItem> storeListItems = new();
            foreach (var customer in customers)
            {
                if (selectedCustomer is not null && customer.Id == selectedCustomer)
                    storeListItems.Add(new SelectListItem(text: customer.Name, value: customer.Id.ToString(), true));
                else
                    storeListItems.Add(new SelectListItem(text: customer.Name, value: customer.Id.ToString()));
            }
            return storeListItems;
        }

        private async Task LoadSellersToViewData(int? selectedSeller = null)
        {
            var sellers = await _userService.GetAllUsersAsync();
            ViewData[ViewConstants.VIEW_SELLERS_SELECT_LIST_ITEMS] = GetSelectListItemsFromSellers(sellers, selectedSeller);
        }

        private IEnumerable<SelectListItem> GetSelectListItemsFromSellers(IEnumerable<UserViewModel> sellers, int? selectedSeller = null)
        {
            List<SelectListItem> sellersListItems = new();
            foreach (var seller in sellers)
            {
                if (selectedSeller is not null && seller.Id == selectedSeller)
                    sellersListItems.Add(new SelectListItem(text: seller.Name, value: seller.Id.ToString(), true));
                else
                    sellersListItems.Add(new SelectListItem(text: seller.Name, value: seller.Id.ToString()));
            }
            return sellersListItems;
        }

        private async Task LoadProductsToViewData()
        {
            var products = await _productService.GetAllProductsAsync();
            ViewData[ViewConstants.VIEW_PRODUCTS_SELECT_LIST_ITEMS] = GetSelectListItemsFromProducts(products);
        }

        private IEnumerable<SelectListItem> GetSelectListItemsFromProducts(IEnumerable<ProductViewModel> products)
        {
            List<SelectListItem> productsListItems = new();
            foreach (var product in products)
            {
                productsListItems.Add(new SelectListItem(text: product.Name, value: product.Id.ToString()));
            }
            return productsListItems;
        }

        protected override void AddErrorsToModelState(ICollection<Error> errors)
        {
            throw new NotImplementedException();
        }
    }
}
