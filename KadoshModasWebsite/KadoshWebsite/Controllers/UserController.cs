using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ValueObjects;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KadoshWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApplicationService _userService;
        private readonly IStoreApplicationService _storeService;

        public UserController(IUserApplicationService userService, IStoreApplicationService storeService)
        {
            _userService = userService;
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            await LoadStoresToViewData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateUserAsync(model);

                if (result.Success)
                {
                    ViewAlerts.SuccessAlert(TempData, result.Message);
                    return RedirectToAction(nameof(Index));
                }

                AddErrorsToModelState(errors: result.Errors);
                await LoadStoresToViewData();
                return View(model);
            }
            else
            {
                await LoadStoresToViewData();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var model = await _userService.GetUserAsync(id.Value);

            await LoadStoresToViewData(model.StoreId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(model);

                if (result.Success)
                {
                    ViewAlerts.SuccessAlert(TempData, result.Message);
                    return RedirectToAction(nameof(Index));
                }

                await LoadStoresToViewData(model.StoreId);
                AddErrorsToModelState(errors: result.Errors);
                return View(model);
            }
            else
            {
                await LoadStoresToViewData(model.StoreId);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var model = await _userService.GetUserAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(UserViewModel model)
        {
            if (!model.Id.HasValue)
                return NotFound();

            var result = await _userService.DeleteUserAsync(model.Id.Value);

            if (result.Success)
            {
                ViewAlerts.SuccessAlert(TempData, result.Message);
                return RedirectToAction(nameof(Index));
            }

            AddErrorsToModelState(errors: result.Errors);
            return View(model);
        }

        private async Task LoadStoresToViewData(int? selectedStore = null)
        {
            var stores = await _storeService.GetAllStoresAsync();
            ViewData[ViewConstants.VIEW_STORE_SELECT_LIST_ITEMS] = GetSelectListItemsFromStores(stores, selectedStore);
        }

        private IEnumerable<SelectListItem> GetSelectListItemsFromStores(IEnumerable<StoreViewModel> stores, int? selectedStore = null)
        {
            List<SelectListItem> storeListItems = new();
            foreach (var store in stores)
            {
                if(selectedStore is not null && store.Id == selectedStore)
                    storeListItems.Add(new SelectListItem(text: store.Name, value: store.Id.ToString(), true));
                else
                    storeListItems.Add(new SelectListItem(text: store.Name, value: store.Id.ToString()));
            }
            return storeListItems;
        }

        private void AddErrorsToModelState(ICollection<Error> errors)
        {
            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_USER_CREATE_COMMAND))
                ModelState.AddModelError(nameof(UserViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_USER_CREATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_USER_UPDATE_COMMAND))
                ModelState.AddModelError(nameof(UserViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_USER_UPDATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_USER_DELETE_COMMAND))
                ModelState.AddModelError(nameof(UserViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_USER_DELETE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_USERNAME_ALREADY_TAKEN))
                ModelState.AddModelError(nameof(UserViewModel.UserName), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_USERNAME_ALREADY_TAKEN));
        }

        private string GetErrorMessagesFromSpecificErrorCode(ICollection<Error> errors, int errorCodeToSearch)
        {
            string errorMessage = string.Empty;
            foreach (var error in errors.Where(x => x.Code == errorCodeToSearch))
            {
                errorMessage += error.Message + ". ";
            }
            return errorMessage;
        }
    }
}
