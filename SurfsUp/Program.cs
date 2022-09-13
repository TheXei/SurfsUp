using LazZiya.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurfsUp.Data;
using SurfsUp.Models;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SurfsUpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SurfsUpContext") ?? throw new InvalidOperationException("Connection string 'SurfsUpContext' not found.")));

builder.Services.AddDbContext<SurfsUpIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SurfsUpIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'SurfsUpIdentityContext' not found.")));

/* Adding the Identity framework to the project. */
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<SurfsUpIdentityContext>();

//Configure paths
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<ITagHelperComponent, LocalizationValidationScriptsTagHelperComponent>();
/* Adding the `IEmailSender` interface to the project. */
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();
var cultureInfo = new CultureInfo("da-DK");
cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
    await SeedData.InitializeUser(services);
}
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


app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
