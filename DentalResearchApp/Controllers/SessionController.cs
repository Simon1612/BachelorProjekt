using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    public class SessionController : Controller
    {
        private readonly IContext _context;

        public SessionController(IContext context)
        {
            _context = context;
        }

        [HttpGet("SessionDetails")]
        public IActionResult SessionDetails(string studyId, string sessionName, string studyName)
        {
            var manager = _context.ManagerFactory.CreateSessionManager();
            var sessionModel = manager.GetStudySession(studyId, sessionName).Result;

            ViewBag.studyName = studyName;

            return View(sessionModel);
        }

        [HttpGet("CreateSession")]
        public IActionResult CreateSession(string studyId, string studyName)
        {
            var sessionModel = new StudySessionModel {StudyId = studyId};
            ViewBag.studyName = studyName;

            return View(sessionModel);
        }

        [HttpPost("CreateSession")]
        public IActionResult CreateSession(StudySessionModel sessionModel)
        {
            var manager = _context.ManagerFactory.CreateSessionManager();
            manager.CreateSession(sessionModel);

            return RedirectToAction("AllStudies","Study");
        }
    }
}