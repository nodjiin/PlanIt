using Microsoft.AspNetCore.Mvc;

namespace PlanIt.Presentation.WebApp.Controllers;
public class ErrorController : Controller
{
    public IActionResult Generic()
    {
        return View();
    }
}
