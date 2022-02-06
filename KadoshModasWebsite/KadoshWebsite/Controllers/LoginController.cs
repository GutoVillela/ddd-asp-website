using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ValueObjects;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserApplicationService _userService;

        public LoginController(IUserApplicationService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IndexAsync(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AuthenticateUserAsync(model.UserName, model.Password);

                if (result.Success)
                {
                    _userService.LoginAuthenticatedUser(model.UserName);
                    return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""));
                }

                AddErrorsToModelState(errors: result.Errors);
                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        protected override void AddErrorsToModelState(ICollection<Error> errors)
        {
            if (errors.Any(x => x.Code == ErrorCodes.ERROR_INVALID_USER_AUTHENTICATE_COMMAND))
                ModelState.AddModelError(nameof(LoginViewModel.UserName), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_INVALID_USER_AUTHENTICATE_COMMAND));

            if (errors.Any(x => x.Code == ErrorCodes.ERROR_AUTHENTICATION_FAILED))
                ModelState.AddModelError(nameof(LoginViewModel.UserName), GetErrorMessagesFromSpecificErrorCode(errors, ErrorCodes.ERROR_AUTHENTICATION_FAILED));
        }
    }
}
