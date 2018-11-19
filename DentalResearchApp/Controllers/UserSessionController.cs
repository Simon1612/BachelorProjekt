using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class UserSessionController : Controller
    {

        private readonly IContext _context;

        public UserSessionController(IContext context)
        {
            _context = context;
        }

        public IActionResult UserSessions(int participantId, int studyId)
        {
            _context.ManagerFactory.CreateSessionManager();

            var viewModel = new UserSessionsViewModel();


            return View();
        }

        [HttpGet("UserSessionDetailsModal")]
        public IActionResult UserSessionDetailsModal() //TODO:Send identifier
        {



            return View();
        }
    }
}