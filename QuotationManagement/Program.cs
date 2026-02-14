using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QuotationManagement.Contexts;



Env.Load(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddAuthentication(options =>
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
        options.ClientId = Environment.GetEnvironmentVariable("SSO__ClientId");
        options.ClientSecret = Environment.GetEnvironmentVariable("SSO__ClientSecret");
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDBContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DBConnectionString")));

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

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "quotation-search",
    defaults: new { controller = "Quotation", action = "Search" },
    pattern: "/quotation/search"
);
app.MapControllerRoute(
    name: "quotation-create",
    defaults: new { controller = "Quotation", action = "Create" },
    pattern: "/quotation/create"
);
app.MapControllerRoute(
    name: "quotation-setup",
    defaults: new { controller = "Quotation", action = "Setup" },
    pattern: "/quotation/setup"
);
app.MapControllerRoute(
    name: "quotation-table",
    defaults: new { controller = "Quotation", action = "Table" },
    pattern: "/quotation/table"
);
app.MapControllerRoute(
    name: "quotation-lifecycle",
    defaults: new { controller = "Quotation", action = "Lifecycle" },
    pattern: "/quotation/lifecycle"
);
app.MapControllerRoute(
    name: "quotation-document",
    defaults: new { controller = "Quotation", action = "Document" },
    pattern: "/quotation/document"
);
app.MapControllerRoute(
    name: "login",
    defaults: new { controller = "Account", action = "Login" },
    pattern: "/account/login"
);
app.MapControllerRoute(
    name: "SSOLogin",
    defaults: new { controller = "Account", action = "SSOLogin" },
    pattern: "/account/SSOLogin"
);
app.MapControllerRoute(
    name: "branch-default-settings",
    defaults: new { controller = "Settings", action = "Default" },
    pattern: "/settings/default"
);
app.MapControllerRoute(
    name: "branch-header-footer-settings",
    defaults: new { controller = "Settings", action = "HeaderFooter" },
    pattern: "/settings/header-footer"
);
app.MapControllerRoute(
    name: "branch-header-footer-settings-create",
    defaults: new { controller = "Settings", action = "HeaderFooterCreate" },
    pattern: "/settings/header-footer/create"
);
app.MapControllerRoute(
    name: "branch-header-footer-settings-edit",
    defaults: new { controller = "Settings", action = "HeaderFooterEdit" },
    pattern: "/settings/header-footer/edit/{settingCode}"
);
app.MapControllerRoute(
    name: "header-footer-templates",
    defaults: new { controller = "Settings", action = "HeaderFooterTemplates" },
    pattern: "/settings/header-footer/templates"
);
app.MapControllerRoute(
    name: "header-footer-templates",
    defaults: new { controller = "Settings", action = "HeaderFooterTemplatesCreate" },
    pattern: "/settings/header-footer/templates/create"
);
app.MapControllerRoute(
    name: "header-footer-templates",
    defaults: new { controller = "Settings", action = "HeaderFooterTemplatesEdit" },
    pattern: "/settings/header-footer/templates/edit/{templateCode}"
);

app.Run();
