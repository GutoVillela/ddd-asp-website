using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace KadoshWebsite.Util
{
    /// <summary>
    /// Helper class to load the SelectList in the informed ViewData in order to use it in the view DropDowns.
    /// </summary>
    public static class SelectListLoaderHelper
    {
        public static async Task LoadStoresToViewData(IStoreApplicationService storeService, ViewDataDictionary viewData, int? selectedStore = null)
        {
            var stores = await storeService.GetAllStoresAsync();
            viewData[ViewConstants.VIEW_STORE_SELECT_LIST_ITEMS] = GetSelectListItemsFromStores(stores, selectedStore);
        }

        private static IEnumerable<SelectListItem> GetSelectListItemsFromStores(IEnumerable<StoreViewModel> stores, int? selectedStore = null)
        {
            List<SelectListItem> storeListItems = new();
            foreach (var store in stores)
            {
                if (selectedStore is not null && store.Id == selectedStore)
                    storeListItems.Add(new SelectListItem(text: store.Name, value: store.Id.ToString(), true));
                else
                    storeListItems.Add(new SelectListItem(text: store.Name, value: store.Id.ToString()));
            }
            return storeListItems;
        }

        public static async Task LoadProductsToViewData(IProductApplicationService productService, ViewDataDictionary viewData)
        {
            var products = await productService.GetAllProductsAsync();
            viewData[ViewConstants.VIEW_PRODUCTS_SELECT_LIST_ITEMS] = GetSelectListItemsFromProducts(products);
        }

        private static IEnumerable<SelectListItem> GetSelectListItemsFromProducts(IEnumerable<ProductViewModel> products)
        {
            List<SelectListItem> productsListItems = new();
            foreach (var product in products)
            {
                productsListItems.Add(new SelectListItem(text: product.Name, value: product.Id.ToString()));
            }
            return productsListItems;
        }

        public static async Task LoadCustomersToViewData(ICustomerApplicationService customerService, ViewDataDictionary viewData, int? selectedCustomer = null)
        {
            var customers = await customerService.GetAllCustomersAsync();
            viewData[ViewConstants.VIEW_CUSTOMERS_SELECT_LIST_ITEMS] = GetSelectListItemsFromCustomers(customers, selectedCustomer);
        }

        private static IEnumerable<SelectListItem> GetSelectListItemsFromCustomers(IEnumerable<CustomerViewModel> customers, int? selectedCustomer = null)
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

        public static async Task LoadSellersToViewData(IUserApplicationService userService, ViewDataDictionary viewData, int? selectedSeller = null)
        {
            var sellers = await userService.GetAllUsersAsync();
            viewData[ViewConstants.VIEW_SELLERS_SELECT_LIST_ITEMS] = GetSelectListItemsFromSellers(sellers, selectedSeller);
        }

        private static IEnumerable<SelectListItem> GetSelectListItemsFromSellers(IEnumerable<UserViewModel> sellers, int? selectedSeller = null)
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
    }
}
