using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/account/login";
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "quotation-search",
    defaults: new { controller = "Quotation", action = "Search" },
    pattern: "/quotation/search");
app.MapControllerRoute(
    name: "quotation-create",
    defaults: new { controller = "Quotation", action = "Create" },
    pattern: "/quotation/create");
app.MapControllerRoute(
    name: "quotation-setup",
    defaults: new { controller = "Quotation", action = "Setup" },
    pattern: "/quotation/setup");
app.MapControllerRoute(
    name: "login",
    defaults: new { controller = "Account", action = "Login" },
    pattern: "/account/login");
app.MapControllerRoute(
    name: "SSOLogin",
    defaults: new { controller = "Account", action = "SSOLogin" },
    pattern: "/account/SSOLogin");


app.Run();
