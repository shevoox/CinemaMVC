using Microsoft.AspNetCore.Mvc;

namespace CinemaMVC.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Search()
        {
            return View();
        }
    }
}
