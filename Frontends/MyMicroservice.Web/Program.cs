using Microsoft.AspNetCore.Authentication.Cookies;
using MyMicroservice.Web.Models;
using MyMicroservice.Web.Services;
using MyMicroservice.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IIdentityService,IdentityService>();

builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetSection("ServiceApiSettings"));


//cookie settings
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
{
    opts.LoginPath = "/Auth/SignIn";
    opts.ExpireTimeSpan = TimeSpan.FromDays(60);
    opts.SlidingExpiration = true; //60 gün içimde  tekrar girildiginde yenilensin mi süre
    opts.Cookie.Name = "mywebcookie";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
