using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Util;
using Microsoft.AspNetCore.Authorization;

namespace KadoshWebsite.Infrastructure
{
    public class AuthorizationManager : AuthorizationHandler<LoggedInAuthorization>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession? _session => _httpContextAccessor.HttpContext?.Session;

        public AuthorizationManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LoggedInAuthorization requirement)
        {
            if (_session is null)
                throw new ApplicationException("Não existe sessão habilitada para validar usuário");

            var loggedInUser = _session.GetString(AuthorizationConstants.LOGGED_IN_USERNAME_SESSION);
            
            if(!string.IsNullOrEmpty(loggedInUser))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
