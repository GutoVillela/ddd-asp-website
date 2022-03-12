using KadoshDomain.Commands.BrandCommands.CreateBrand;
using KadoshDomain.Commands.BrandCommands.DeleteBrand;
using KadoshDomain.Commands.BrandCommands.UpdateBrand;
using KadoshDomain.Commands.CategoryCommands.CreateCategory;
using KadoshDomain.Commands.CategoryCommands.DeleteCategory;
using KadoshDomain.Commands.CategoryCommands.UpdateCategory;
using KadoshDomain.Commands.CustomerCommands.CreateCustomer;
using KadoshDomain.Commands.CustomerCommands.DeleteCustomer;
using KadoshDomain.Commands.CustomerCommands.InformPayment;
using KadoshDomain.Commands.CustomerCommands.UpdateCustomer;
using KadoshDomain.Commands.ProductCommands.CreateProduct;
using KadoshDomain.Commands.ProductCommands.DeleteProduct;
using KadoshDomain.Commands.ProductCommands.UpdateProduct;
using KadoshDomain.Commands.SaleCommands.CreateSaleInCash;
using KadoshDomain.Commands.SaleCommands.CreateSaleInInstallments;
using KadoshDomain.Commands.SaleCommands.CreateSaleOnCredit;
using KadoshDomain.Commands.SaleCommands.PayOffSale;
using KadoshDomain.Commands.StoreCommands.CreateStore;
using KadoshDomain.Commands.StoreCommands.DeleteStore;
using KadoshDomain.Commands.StoreCommands.UpdateStore;
using KadoshDomain.Commands.UserCommands.AuthenticateUser;
using KadoshDomain.Commands.UserCommands.CreateUser;
using KadoshDomain.Commands.UserCommands.DeleteUser;
using KadoshDomain.Commands.UserCommands.UpdateUser;
using KadoshDomain.Repositories;
using KadoshRepository.Persistence.DataContexts;
using KadoshRepository.Repositories;
using KadoshRepository.UnitOfWork;
using KadoshShared.Handlers;
using KadoshShared.Repositories;
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
builder.Services.AddScoped<ICategoryApplicationService, CategoryApplicationService>();
builder.Services.AddScoped<IProductApplicationService, ProductApplicationService>();
builder.Services.AddScoped<ISaleApplicationService, SaleApplicationService>();
builder.Services.AddScoped<ICustomerPostingApplicationService, CustomerPostingApplicationService>();

// Unit of Work Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Domain Repositories Injections
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISaleInCashRepository, SaleInCashRepository>();
builder.Services.AddScoped<ISaleInInstallmentsRepository, SaleInInstallmentsRepository>();
builder.Services.AddScoped<ISaleOnCreditRepository, SaleOnCreditRepository>();
builder.Services.AddScoped<ISaleItemRepository, SaleItemRepository>();
builder.Services.AddScoped<ICustomerPostingRepository, CustomerPostingRepository>();
builder.Services.AddScoped<IInstallmentRepository, InstallmentRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// Command Handlers
builder.Services.AddScoped<IHandler<CreateBrandCommand>, CreateBrandHandler>();
builder.Services.AddScoped<IHandler<DeleteBrandCommand>, DeleteBrandHandler>();
builder.Services.AddScoped<IHandler<UpdateBrandCommand>, UpdateBrandHandler>();
builder.Services.AddScoped<IHandler<CreateCategoryCommand>, CreateCategoryHandler>();
builder.Services.AddScoped<IHandler<DeleteCategoryCommand>, DeleteCategoryHandler>();
builder.Services.AddScoped<IHandler<UpdateCategoryCommand>, UpdateCategoryHandler>();
builder.Services.AddScoped<IHandler<CreateCustomerCommand>, CreateCustomerHandler>();
builder.Services.AddScoped<IHandler<DeleteCustomerCommand>, DeleteCustomerHandler>();
builder.Services.AddScoped<IHandler<UpdateCustomerCommand>, UpdateCustomerHandler>();
builder.Services.AddScoped<IHandler<InformPaymentCommand>, InformPaymentHandler>();
builder.Services.AddScoped<IHandler<CreateProductCommand>, CreateProductHandler>();
builder.Services.AddScoped<IHandler<DeleteProductCommand>, DeleteProductHandler>();
builder.Services.AddScoped<IHandler<UpdateProductCommand>, UpdateProductHandler>();
builder.Services.AddScoped<IHandler<CreateSaleInCashCommand>, CreateSaleInCashHandler>();
builder.Services.AddScoped<IHandler<CreateSaleInInstallmentsCommand>, CreateSaleInInstallmentsHandler>();
builder.Services.AddScoped<IHandler<CreateSaleOnCreditCommand>, CreateSaleOnCreditHandler>();
builder.Services.AddScoped<IHandler<PayOffSaleCommand>, PayOffSaleHandler>();
builder.Services.AddScoped<IHandler<CreateStoreCommand>, CreateStoreHandler>();
builder.Services.AddScoped<IHandler<DeleteStoreCommand>, DeleteStoreHandler>();
builder.Services.AddScoped<IHandler<UpdateStoreCommand>, UpdateStoreHandler>();
builder.Services.AddScoped<IHandler<AuthenticateUserCommand>, AuthenticateUserHandler>();
builder.Services.AddScoped<IHandler<CreateUserCommand>, CreateUserHandler>();
builder.Services.AddScoped<IHandler<DeleteUserCommand>, DeleteUserHandler>();
builder.Services.AddScoped<IHandler<UpdateUserCommand>, UpdateUserHandler>();

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