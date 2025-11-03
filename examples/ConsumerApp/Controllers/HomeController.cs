using Microsoft.AspNetCore.Mvc;

namespace ConsumerApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Message = "Welcome to the Odin Consumer Demo App!";
        return View();
    }
}