using Galaxon.Astronomy.SpaceCalendars.com.Repositories;
using Galaxon.Astronomy.SpaceCalendars.com.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Galaxon.Astronomy.SpaceCalendars.com;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder);
        WebApplication app = builder.Build();
        ConfigureApp(app);
        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder webAppBuilder)
    {
        webAppBuilder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
        webAppBuilder.Services.AddScoped<DocumentService>();
        webAppBuilder.Services.AddScoped<BufferedFileUploadService>();
        webAppBuilder.Services.AddScoped<MessageBoxService>();

        webAppBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                webAppBuilder.Configuration.GetConnectionString("SpaceCalendars"));
        });

        webAppBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();

        webAppBuilder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        webAppBuilder.Services.AddControllersWithViews(options =>
            options.Filters.Add(new AuthorizeFilter()));
    }

    private static void ConfigureApp(WebApplication webApp)
    {
        // Configure the HTTP request pipeline.
        if (webApp.Environment.IsDevelopment())
        {
            webApp.UseDeveloperExceptionPage();
            webApp.UseMigrationsEndPoint();
        }
        else
        {
            webApp.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios,
            // see https://aka.ms/aspnetcore-hsts.
            webApp.UseHsts();
        }

        webApp.UseHttpsRedirection();
        webApp.UseStaticFiles();

        webApp.UseRouting();

        webApp.UseAuthentication();
        webApp.UseAuthorization();

        webApp.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        webApp.MapRazorPages();
        webApp.MapFallbackToController(@"{**alias}", "DisplayFromPathAlias", "Document");
    }
}
