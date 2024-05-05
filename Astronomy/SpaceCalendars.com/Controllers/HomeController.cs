using System.Diagnostics;
using Galaxon.Astronomy.SpaceCalendars.com.Repositories;
using Galaxon.Astronomy.SpaceCalendars.com.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.SpaceCalendars.com.Controllers;

public class HomeController(ILogger<HomeController> logger, IDocumentRepository documentRepo) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;

    [AllowAnonymous]
    public ViewResult Index()
    {
        // // Check we have a welcome page.
        // Document? doc = documentRepo.GetByTitle("Welcome");
        // if (doc != null)
        // {
        //     return Redirect("/welcome");
        // }

        ViewBag.PageTitle = "Placeholder for Welcome page.";
        return View();
    }

    [AllowAnonymous]
    public ViewResult Privacy()
    {
        ViewBag.PageTitle = "Privacy Policy";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public ViewResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
