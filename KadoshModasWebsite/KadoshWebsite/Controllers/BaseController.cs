using KadoshShared.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace KadoshWebsite.Controllers
{
    public abstract class BaseController : Controller
    {
        protected abstract void AddErrorsToModelState(ICollection<Error> errors);

        protected string GetErrorMessagesFromSpecificErrorCode(ICollection<Error> errors, int errorCodeToSearch)
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
