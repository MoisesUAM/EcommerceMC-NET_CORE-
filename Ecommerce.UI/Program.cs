using Ecommerce.BLL.Utilities;
using Ecommerce.BLL.Utilities.Implementations;
using Ecommerce.BLL.Utilities.Interfaces;
using Ecommerce.DAL.Data;
using Ecommerce.DAL.InitConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("EcommerceDB") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUnitWork, UnitWorkImpl>();
builder.Services.AddScoped<IDbInitializer, DbInitializerImpl>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddSession(options =>
{
    _ = options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();
app.UseSession();

//Para usar la autenticacion y autorizacion por roles debe estar estas lineas en ese orden importante!
app.UseAuthentication();
app.UseAuthorization();

//Aplicar migraciones y datos iniciales
using(var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        var init = service.GetRequiredService<IDbInitializer>();
        init.Initialize();
    }
    catch (Exception e)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(e, "Ocurrio un error al ejecutar las migraciones y datos iniciales");
    }
}

app.MapControllerRoute(
    name: "areas",
    pattern: "{area=Inventory}/{controller=Home}/{action=Index}/{id?}"
    );
app.MapRazorPages();
IWebHostEnvironment env = app.Environment;
Rotativa.AspNetCore.RotativaConfiguration.Setup(env.WebRootPath, "..\\Rotativa\\Windows");
app.Run();
