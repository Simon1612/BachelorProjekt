using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}