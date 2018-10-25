using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class ProfileController : Controller
    {
        private readonly IContext _context;

        public ProfileController(IContext context)
        {
            _context = context;
        }

        public IActionResult Profile()
        {
            
            var manager = _context.ManagerFactory.CreateUserManager();
            return View();
        }
    }
}