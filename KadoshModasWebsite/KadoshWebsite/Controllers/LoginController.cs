using KadoshShared.Constants.ErrorCodes;
using KadoshShared.ValueObjects;
using KadoshWebsite.Infrastructure.Authentication;
using KadoshWebsite.Models;
using KadoshWebsite.Services.Interfaces;
using KadoshWebsite.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KadoshWebsite.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserApplicationService _userService;
        private readonly IStoreApplicationService _storeApplicationService;
        private readonly ITokenService _tokenService;

        public LoginController(IUserApplicationService userService, IStoreApplicationService storeApplicationService, ITokenService tokenService)
        {
            _userService = userService;
            _storeApplicationService = storeApplicationService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //TODO Replace this for Seeding service
            var users = await _userService.GetAllUsersAsync();
            if (!users.Any())
            {
                StoreViewModel model = new()
                {
                    Name = "Kadosh Modas",
                    Street = "R. Valença do Minho",
                    City = "São Paulo",
                    Number = "413"
                };

                var resultStore = await _storeApplicationService.CreateStoreAsync(model);

                UserViewModel user = new()
                {
                    Name = "Administrador",
                    UserName = "admin",
                    Password = "admin",
                    Role = KadoshDomain.Enums.EUserRole.Administrator,
                    StoreId = 1
                };
                var result = await _userService.CreateUserAsync(user);
            }

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

        [HttpPost]
        public async Task<IActionResult> AuthenticateCustomerAsync([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AuthenticateUserAsync(model.UserName, model.Password);

                if (!result.Success)
                {
                    if (result.Errors.Any(x => x.Code == ErrorCodes.ERROR_USERNAME_NOT_FOUND))
                        return NotFound(result.Message);
                    else
                        return BadRequest(result.Message);
                }

                var token = _tokenService.GenerateToken(model.UserName, Roles.CUSTOMER_ROLE);//Authenticate Customer Role
                var refreshToken = _tokenService.GenerateRefreshToken();
                _tokenService.SaveRefreshToken(new RefreshToken(model.UserName, token));

                model.Password = string.Empty;
                return Ok(new
                {
                    token,
                    refreshToken,
                    user = model
                });
            }
            else
            {
                return BadRequest(model);
            }
        }

        [HttpPost]
        public IActionResult RefreshToken([FromBody] string token, string refreshToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var savedRefreshToken = _tokenService.GetRefreshToken(username);
            if (savedRefreshToken != refreshToken)
                throw new SecurityTokenException("Invalid refresh token");

            var newJwtToken = _tokenService.GenerateToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            _tokenService.DeleteRefreshToken(new RefreshToken(username, refreshToken));
            _tokenService.SaveRefreshToken(new RefreshToken(username, newRefreshToken));

            return new ObjectResult(new
            {
                token = newJwtToken,
                refreshToken = newRefreshToken
            });
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
