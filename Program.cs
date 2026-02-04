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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "/",
        defaults: new { controller = "Home", action = "Index" });
    endpoints.MapControllerRoute(
        name: "about",
        pattern: "/about/",
        defaults: new { controller = "Home", action = "About" });
    endpoints.MapControllerRoute(
                    name: "form-employee-details",
                    pattern: "/employee-details/{id?}",
                    defaults: new { controller = "Home", action = "LoadEmploymentInfo" });
    endpoints.MapControllerRoute(
                   name: "form-loan-details",
                   pattern: "/loan-details/{id?}",
                   defaults: new { controller = "Home", action = "LoadLoanInfo" });
    endpoints.MapControllerRoute(
                   name: "form-personal-details",
                   pattern: "/personal-details/{id?}",
                   defaults: new { controller = "Home", action = "LoadPersonalInfo" });
    endpoints.MapControllerRoute(
                    name: "sign-document",
                    pattern: "/sign-document/{id?}",
                    defaults: new { controller = "Home", action = "SignDocument" });
    endpoints.MapControllerRoute(
                    name: "thank-you",
                    pattern: "/thank-you/",
                    defaults: new { controller = "Home", action = "SignCompleted" });
    endpoints.MapControllerRoute(
        name: "download-document",
        pattern: "/download/",
        defaults: new { controller = "Home", action = "DownloadDocument" });
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
