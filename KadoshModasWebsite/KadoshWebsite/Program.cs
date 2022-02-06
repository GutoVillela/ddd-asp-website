using KadoshDomain.Repositories;
using KadoshDomain.Services;
using KadoshDomain.Services.Interfaces;
using KadoshRepository.Persistence.DataContexts;
using KadoshRepository.Repositories;
using KadoshWebsite.Services;
using KadoshWebsite.Services.Interfaces;
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

// Domain Services Injection
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IUserService, UserService>();

// Domain Repositories Injections
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}");

app.Run();
