using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize(Roles = "Administrator, Researcher")]
    public class StudyController : Controller
    {

        private readonly IContext _context;

        public StudyController(IContext context)
        {
            _context = context;
        }

        [HttpGet("AllStudies")]
        public IActionResult AllStudies()
        {
            var manager = _context.ManagerFactory.CreateExternalDbManager();

            var studies = new AllStudiesViewModel{AllStudies = manager.GetAllStudyListModels()};

            return View(studies);
        }

        [HttpGet("StudyDetails")]
        public IActionResult StudyDetails(int id)
        {
            var sessionManager = _context.ManagerFactory.CreateSessionManager();
            var extManager = _context.ManagerFactory.CreateExternalDbManager();
            var participants = extManager.GetParticipantIds(id);

            var sessions = sessionManager.GetAllStudySessionsNamesForStudy(id);
            var study = extManager.GetStudy(id);

            var viewModel = new StudyDetailsViewModel
            {
                StudyId = study.StudyId,
                StudyDescription = study.Description,
                StudyName = study.StudyName,
                Participants = participants,
                Sessions = sessions
            };

            return View(viewModel);
        }
    }
}