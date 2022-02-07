using KadoshDomain.Repositories;
using KadoshDomain.Services;
using KadoshDomain.Services.Interfaces;
using KadoshRepository.Persistence.DataContexts;
using KadoshRepository.Repositories;
using KadoshWebsite.Infrastructure;
using KadoshWebsite.Infrastructure.Authorization;
using KadoshWebsite.Services;
using KadoshWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add repository
string connectionString = builder.Configuration.GetConnectionString("AppMySQLDB");
builder.Services.AddDbContext<StoreDataContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Application Services Injection
builder.Services.AddScoped<IStoreApplicationService, StoreApplicationService>();
builder.Services.AddScoped<ICustomerApplicationService, CustomerApplicationService>();
builder.Services.AddScoped<IUserApplicationService, UserApplicationService>();
builder.Services.AddScoped<IBrandApplicationService, BrandApplicationService>();

// Domain Services Injection
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBrandService, BrandService>();

// Domain Repositories Injections
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();

// HttpContext
builder.Services.AddHttpContextAccessor();

// Authorization Handler
builder.Services.AddSingleton<IAuthorizationHandler, AuthorizationManager>();

// Add Authorizations Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(nameof(LoggedInAuthorization), policy => policy.Requirements.Add(new LoggedInAuthorization()));
});

// Sessions
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// Authorization Scheme (must come before app.UseAuthorization() )
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}");



app.Run();
