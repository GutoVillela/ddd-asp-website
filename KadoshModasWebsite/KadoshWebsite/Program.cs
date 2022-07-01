using Kadosh.LegacyRepository.Repositories;
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
using KadoshDomain.Commands.SaleCommands.InformPayment;
using KadoshDomain.Commands.SaleCommands.PayOffInstallment;
using KadoshDomain.Commands.SaleCommands.PayOffSale;
using KadoshDomain.Commands.SettingsCommands.ImportDataFromLegacy;
using KadoshDomain.Commands.StoreCommands.CreateStore;
using KadoshDomain.Commands.StoreCommands.DeleteStore;
using KadoshDomain.Commands.StoreCommands.UpdateStore;
using KadoshDomain.Commands.UserCommands.AuthenticateUser;
using KadoshDomain.Commands.UserCommands.CreateUser;
using KadoshDomain.Commands.UserCommands.DeleteUser;
using KadoshDomain.Commands.UserCommands.UpdateUser;
using KadoshDomain.Entities;
using KadoshDomain.LegacyEntities;
using KadoshDomain.Queries.BrandQueries.GetAllBrands;
using KadoshDomain.Queries.BrandQueries.GetBrandById;
using KadoshDomain.Queries.CategoryQueries.GetAllCategories;
using KadoshDomain.Queries.CategoryQueries.GetCategoryById;
using KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromCustomer;
using KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromDate;
using KadoshDomain.Queries.CustomerPostingQueries.GetAllPostingsFromSale;
using KadoshDomain.Queries.CustomerQueries.GetAllCustomers;
using KadoshDomain.Queries.CustomerQueries.GetAllDelinquentCustomers;
using KadoshDomain.Queries.CustomerQueries.GetCustomerById;
using KadoshDomain.Queries.CustomerQueries.GetCustomersByName;
using KadoshDomain.Queries.CustomerQueries.GetCustomerTotalDebt;
using KadoshDomain.Queries.ProductQueries.GetAllProducts;
using KadoshDomain.Queries.ProductQueries.GetProductByBarCode;
using KadoshDomain.Queries.ProductQueries.GetProductById;
using KadoshDomain.Queries.ProductQueries.GetProductsByName;
using KadoshDomain.Queries.SaleQueries.GetAllOpenSales;
using KadoshDomain.Queries.SaleQueries.GetAllSales;
using KadoshDomain.Queries.SaleQueries.GetAllSalesByCustomerId;
using KadoshDomain.Queries.SaleQueries.GetSaleById;
using KadoshDomain.Queries.SaleQueries.GetSalesByDate;
using KadoshDomain.Queries.SaleQueries.GetSalesOfTheWeek;
using KadoshDomain.Queries.StoreQueries.GetAllStores;
using KadoshDomain.Queries.StoreQueries.GetStoreById;
using KadoshDomain.Queries.UserQueries.GetAllUsers;
using KadoshDomain.Queries.UserQueries.GetUserById;
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
using Rotativa.AspNetCore;

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
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISettingsApplicationService, SettingsApplicationService>();

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

// Legacy Repositories Injection
builder.Services.AddScoped<ILegacyRepository<Brand, BrandLegacy>, BrandLegacyRepository>();
builder.Services.AddScoped<ILegacyRepository<Category, CategoryLegacy>, CategoryLegacyRepository>();
builder.Services.AddScoped<ILegacyRepository<Product, ProductLegacy>, ProductLegacyRepository>();
builder.Services.AddScoped<ILegacyRepository<Customer, CustomerLegacy>, CustomerLegacyRepository>();
builder.Services.AddScoped<ILegacyRepository<Sale, SaleLegacy>, SaleLegacyRepository>();
builder.Services.AddScoped<ILegacyRepository<CustomerPosting, CustomerPostingLegacy>, CustomerPostingLegacyRepository>();

// Command Handlers
builder.Services.AddScoped<ICommandHandler<CreateBrandCommand>, CreateBrandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteBrandCommand>, DeleteBrandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateBrandCommand>, UpdateBrandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateCategoryCommand>, CreateCategoryHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteCategoryCommand>, DeleteCategoryHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateCategoryCommand>, UpdateCategoryHandler>();
builder.Services.AddScoped<ICommandHandler<CreateCustomerCommand>, CreateCustomerHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteCustomerCommand>, DeleteCustomerHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateCustomerCommand>, UpdateCustomerHandler>();
builder.Services.AddScoped<ICommandHandler<InformPaymentCommand>, InformPaymentHandler>();
builder.Services.AddScoped<ICommandHandler<CreateProductCommand>, CreateProductHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteProductCommand>, DeleteProductHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateProductCommand>, UpdateProductHandler>();
builder.Services.AddScoped<ICommandHandler<CreateSaleInCashCommand>, CreateSaleInCashHandler>();
builder.Services.AddScoped<ICommandHandler<CreateSaleInInstallmentsCommand>, CreateSaleInInstallmentsHandler>();
builder.Services.AddScoped<ICommandHandler<CreateSaleOnCreditCommand>, CreateSaleOnCreditHandler>();
builder.Services.AddScoped<ICommandHandler<PayOffSaleCommand>, PayOffSaleHandler>();
builder.Services.AddScoped<ICommandHandler<CreateStoreCommand>, CreateStoreHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteStoreCommand>, DeleteStoreHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateStoreCommand>, UpdateStoreHandler>();
builder.Services.AddScoped<ICommandHandler<AuthenticateUserCommand>, AuthenticateUserHandler>();
builder.Services.AddScoped<ICommandHandler<CreateUserCommand>, CreateUserHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteUserCommand>, DeleteUserHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateUserCommand>, UpdateUserHandler>();
builder.Services.AddScoped<ICommandHandler<InformSalePaymentCommand>, InformSalePaymentHandler>();
builder.Services.AddScoped<ICommandHandler<PayOffInstallmentCommand>, PayOffInstallmentHandler>();
builder.Services.AddScoped<ICommandHandler<ImportDataFromLegacyCommand>, ImportDataFromLegacyHandler>();

// Query Handlers
builder.Services.AddScoped<IQueryHandler<GetAllBrandsQuery, GetAllBrandsQueryResult>, GetAllBrandsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetBrandByIdQuery, GetBrandByIdQueryResult>, GetBrandByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllCategoriesQuery, GetAllCategoriesQueryResult>, GetAllCategoriesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult>, GetCategoryByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllCustomersQuery, GetAllCustomersQueryResult>, GetAllCustomersQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCustomerByIdQuery, GetCustomerByIdQueryResult>, GetCustomerByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCustomerTotalDebtQuery, GetCustomerTotalDebtQueryResult>, GetCustomerTotalDebtQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllPostingsFromCustomerQuery, GetAllPostingsFromCustomerQueryResult>, GetAllPostingsFromCustomerQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllProductsQuery, GetAllProductsQueryResult>, GetAllProductsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetProductByIdQuery, GetProductByIdQueryResult>, GetProductByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllSalesQuery, GetAllSalesQueryResult>, GetAllSalesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllSalesByCustomerIdQuery, GetAllSalesByCustomerIdQueryResult>, GetAllSalesByCustomerIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllStoresQuery, GetAllStoresQueryResult>, GetAllStoresQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetStoreByIdQuery, GetStoreByIdQueryResult>, GetStoreByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllUsersQuery, GetAllUsersQueryResult>, GetAllUsersQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetUserByIdQuery, GetUserByIdQueryResult>, GetUserByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCustomersByNameQuery, GetCustomersByNameQueryResult>, GetCustomersByNameQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetSaleByIdQuery, GetSaleByIdQueryResult>, GetSaleByIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllPostingsFromSaleQuery, GetAllPostingsFromSaleQueryResult>, GetAllPostingsFromSaleQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllPostingsFromStoreAndDateQuery, GetAllPostingsFromStoreAndDateQueryResult>, GetAllPostingsFromStoreAndDateQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetSalesOfTheWeekQuery, GetSalesOfTheWeekQueryResult>, GetSalesOfTheWeekQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllDelinquentCustomersQuery, GetAllDelinquentCustomersQueryResult>, GetAllDelinquentCustomersQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllOpenSalesQuery, GetAllOpenSalesQueryResult>, GetAllOpenSalesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetSalesByDateQuery, GetSalesByDateQueryResult>, GetSalesByDateQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetProductsByNameQuery, GetProductsByNameQueryResult>, GetProductsByNameQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetProductByBarCodeQuery, GetProductByBarCodeQueryResult>, GetProductByBarCodeQueryHandler>();

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

// Service Provider Manager
ServiceProviderManager.SetServiceProvider(builder.Services.BuildServiceProvider());

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

// Rotativa
RotativaConfiguration.Setup(app.Environment.WebRootPath);

app.Run();