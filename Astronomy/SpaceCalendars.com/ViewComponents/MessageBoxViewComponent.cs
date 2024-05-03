using Galaxon.Astronomy.SpaceCalendars.com.Models;
using Microsoft.AspNetCore.Mvc;

namespace Galaxon.Astronomy.SpaceCalendars.com.ViewComponents;

public class MessageBoxViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        MessageBox? messageBox = TempData["MessageBox"] is string json
            ? MessageBox.Deserialize(json) 
            : null;
        return View(messageBox);
    }
}
