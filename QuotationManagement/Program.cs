var builder = WebApplication.CreateBuilder(args);

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


app.Run();
