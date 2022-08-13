using Microsoft.AspNetCore.Mvc;

namespace PlanIt.Presentation.WebApp.Controllers;
public class PlanController : Controller
{
    public IActionResult Create()
    {
        return View();
    }
}
