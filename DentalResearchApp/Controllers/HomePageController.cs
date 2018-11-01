using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class HomePageController : Controller
    {
        public ActionResult HomePage()
        {
            return View();
        }
    }
}