using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class HomePageController : Controller
    {
        [HttpGet]
        public ActionResult HomePage()
        {
            return View();
        }
    }
}