using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ValueObjects;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    public class StoreController : Controller
    {
        private readonly IStoreApplicationService _service;

        public StoreController(IStoreApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var stores = await _service.GetAllStoresAsync();
            return View(stores);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(StoreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateStoreAsync(model);

                if(result.Success)
                    return RedirectToAction(nameof(Index));

                AddErrorsToModelState(errors: result.Errors);
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int? id)
        {
            if(!id.HasValue)
                return NotFound();

            var model = await _service.GetStoreAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(StoreViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateStoreAsync(model);

                if (result.Success)
                    return RedirectToAction(nameof(Index));

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

            var model = await _service.GetStoreAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(StoreViewModel model)
        {
            if (!model.Id.HasValue)
                return NotFound();

            await _service.DeleteStoreAsync(model.Id.Value);
            return RedirectToAction(nameof(Index));
        }

        private void AddErrorsToModelState(ICollection<Error> errors)
        {
            if (errors.Any(x => x.Code == ErrorCodes.ERROR_REPEATED_STORE_ADDRESS))
                ModelState.AddModelError(nameof(StoreViewModel.Street), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_REPEATED_STORE_ADDRESS));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_STORE_CREATE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_STORE_CREATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_STORE_UPDATE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_STORE_UPDATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_STORE_DELETE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_STORE_DELETE_COMMAND));
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
