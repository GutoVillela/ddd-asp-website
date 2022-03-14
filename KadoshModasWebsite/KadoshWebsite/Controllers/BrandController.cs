using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ValueObjects;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    [Authorize(Policy = nameof(LoggedInAuthorization))]
    public class BrandController : BaseController
    {
        private readonly IBrandApplicationService _service;

        public BrandController(IBrandApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int? page)
        {

            var brands = await _service.GetAllBrandsPaginatedAsync(page ?? 1, PaginationManager.PAGE_SIZE);
            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(BrandViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateBrandAsync(model);

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
            if (id is null)
                return NotFound();

            var model = await _service.GetBrandAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(BrandViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateBrandAsync(model);

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

            var model = await _service.GetBrandAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(UserViewModel model)
        {
            if (!model.Id.HasValue)
                return NotFound();

            var result = await _service.DeleteBrandAsync(model.Id.Value);

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
            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_BRAND_CREATE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_BRAND_CREATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_BRAND_UPDATE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_BRAND_UPDATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_BRAND_DELETE_COMMAND))
                ModelState.AddModelError(nameof(StoreViewModel.Name), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_BRAND_DELETE_COMMAND));
        }
    }
}
