using KadoshShared.Constants.ErrorCodes;
using KadoshShared.Constants.QueriesMessages;
using KadoshShared.ValueObjects;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Infrastructure.Authentication;
using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ICustomerApplicationService _service;

        public CustomerController(ICustomerApplicationService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> IndexAsync(int? page, string? queryByName, bool? includeInactive)
        {
            PaginatedListViewModel<CustomerViewModel> customers;
            
            if(string.IsNullOrEmpty(queryByName))
                customers = await _service.GetAllCustomersPaginatedAsync(page ?? 1, PaginationManager.PAGE_SIZE, includeInactive ?? false);
            else
                customers = await _service.GetAllCustomersByNamePaginatedAsync(queryByName, page ?? 1, PaginationManager.PAGE_SIZE, includeInactive ?? false);

            ViewData["QueryByName"] = queryByName;
            ViewData["IncludeInactive"] = includeInactive ?? false;
            return View(customers);
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
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
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var model = await _service.GetCustomerAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
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
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var model = await _service.GetCustomerAsync(id.Value);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
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

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var model = await _service.GetCustomerAsync(id.Value);
            model.TotalDebt = await GetCustomerTotalDebtAsync(id.Value);

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<decimal> GetCustomerTotalDebtAsync(int? customerId)
        {
            ArgumentNullException.ThrowIfNull(customerId);

            var totalDebt = await _service.GetCustomerTotalDebtAsync(customerId.Value);

            return totalDebt;
        }

        [HttpPost]
        [Authorize(Policy = nameof(LoggedInAuthorization))]
        public async Task<IActionResult> InformPaymentAsync(int? customerId, decimal? amountToInform)
        {
            ArgumentNullException.ThrowIfNull(customerId);
            ArgumentNullException.ThrowIfNull(amountToInform);

            var result = await _service.InformCustomerPaymentAsync(customerId.Value, amountToInform.Value);

            if (result.Success)
                return Ok(result.Message);

            return BadRequest(result.Message);
        }

        [HttpGet]
        [Authorize(Roles = Roles.CUSTOMER_ROLE)]
        public async Task<IActionResult> GetCustomerByUsernameAsync([FromQuery] string username)
        {
            try
            {
                var customer = await _service.GetCustomerUserByUsernameAsync(username);

                if (customer is null)
                {
                    return NotFound();
                }

                return Ok(new
                {
                    customer
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerByIdAsync([FromQuery] int id)
        {
            try
            {
                var model = await _service.GetCustomerAsync(id);
                model.TotalDebt = await GetCustomerTotalDebtAsync(id);

                return Ok(model);
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains(CustomerQueriesMessages.ERROR_CUSTOMER_ID_NOT_FOUND))
                    return NotFound(ex.Message);

                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerUserAsync([FromBody] CustomerUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (model.CustomerId<= 0 || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                    return BadRequest();

                var result = await _service.CreateCustomerUserAsync(model.CustomerId, model.Username, model.Password);

                if (!result.Success)
                {
                    return Conflict(result.Message);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
