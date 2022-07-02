using KadoshWebsite.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KadoshWebsite.Infrastructure
{
    public static class Settings
    {
        public static string Secret => "b090cbafec8b4f46bfb75c7db85ea7fdbe59136c05ac4ae09bea0a1654a5300d906d0f943bf64b15b530aacd13481495";

        public static void ConfigureAuthentication(WebApplicationBuilder builder)
        {
            var key = Encoding.ASCII.GetBytes(Secret);

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public static void ConfigureAuthorization(WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                var authAttr = endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>();
                if (authAttr is not null && authAttr.Policy == nameof(LoggedInAuthorization))
                {
                    var authService = context.RequestServices.GetRequiredService<IAuthorizationService>();
                    var result = await authService.AuthorizeAsync(context.User, context.GetRouteData(), authAttr.Policy);
                    if (!result.Succeeded)
                    {
                        var path = $"/Login";
                        context.Response.Redirect(path);
                        return;
                    }
                }
                await next();
            });
        }
    }
}
