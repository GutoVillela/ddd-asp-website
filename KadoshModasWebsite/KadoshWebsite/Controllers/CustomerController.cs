using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ValueObjects;
using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    [Authorize(Policy = nameof(LoggedInAuthorization))]
    public class CustomerController : BaseController
    {
        private readonly ICustomerApplicationService _service;

        public CustomerController(ICustomerApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var customers = await _service.GetAllCustomersAsync();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateCustomerAsync(model);

                if (result.Success)
                {
                    ViewAlerts.SuccessAlert(TempData, result.Message);
                    return RedirectToAction(nameof(Index));
                }

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
            if (!id.HasValue)
                return NotFound();

            var model = await _service.GetCustomerAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateCustomerAsync(model);

                if (result.Success)
                {
                    ViewAlerts.SuccessAlert(TempData, result.Message);
                    return RedirectToAction(nameof(Index));
                }

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

            var model = await _service.GetCustomerAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(StoreViewModel model)
        {
            if (!model.Id.HasValue)
                return NotFound();

            var result = await _service.DeleteCustomerAsync(model.Id.Value);

            if (result.Success)
            {
                ViewAlerts.SuccessAlert(TempData, result.Message);
                return RedirectToAction(nameof(Index));
            }

            AddErrorsToModelState(errors: result.Errors);
            return View(model);
        }

        protected override void AddErrorsToModelState(ICollection<Error> errors)
        {
            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_CUSTOMER_CREATE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_CUSTOMER_CREATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_CUSTOMER_UPDATE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_CUSTOMER_UPDATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_CUSTOMER_DELETE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_CUSTOMER_DELETE_COMMAND));
        }
    }
}
