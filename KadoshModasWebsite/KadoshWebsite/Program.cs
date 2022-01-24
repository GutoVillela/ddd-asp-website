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

// Domain Services Injection
builder.Services.AddScoped<IStoreService, StoreService>();

// Domain Repositories Injections
builder.Services.AddScoped<IStoreRepository, StoreRepository>();

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
    pattern: "{controller=Store}/{action=Index}/{id?}");

app.Run();
