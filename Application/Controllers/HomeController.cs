using Microsoft.AspNetCore.Mvc;

[Controller]
public class HomeController : Controller
{
    [Route("home/index")]
    public IActionResult Index()
    {
        ViewBag.Title = "HomeController";
        return View();
    }
}