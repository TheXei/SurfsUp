using BlazorApp1.Areas.Identity;
using BlazorApp1.Data;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;
using SurfsUp.Models;
using SurfsUp.Utility;
using Microsoft.AspNetCore.ResponseCompression;
using BlazorApp1.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SurfsUpIdentityContextConnection");

builder.Services.AddDbContext<SurfsUpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SurfsUpContext") ?? throw new InvalidOperationException("Connection string 'SurfsUpContext' not found."), b => b.MigrationsAssembly("Data")));

builder.Services.AddDbContext<SurfsUpIdentityContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Data")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<SurfsUpIdentityContext>();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication()
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

/* Adding the `IEmailSender` interface to the project. */
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7277/api/") });
builder.Services.AddBlazoredSessionStorage();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
    await SeedData.InitializeUser(services);
}

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseIPRateLimiting();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapHub<ChatHub>("/chathub");
app.MapFallbackToPage("/_Host");

app.Run();
