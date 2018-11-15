using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class StudyController : Controller
    {

        private readonly IContext _context;

        public StudyController(IContext context)
        {
            _context = context;
        }

        [HttpGet("AllStudies")]
        public ActionResult AllStudies()
        {
            var manager = _context.ManagerFactory.CreateExternalDbManager();

            var studies = new AllStudiesViewModel{AllStudies = manager.GetAllStudyListModels()};

            return View(studies);
        }

        [HttpGet("StudyDetails")]
        public ActionResult StudyDetails(int id)
        {
            var sessionManager = _context.ManagerFactory.CreateSessionManager();
            var extManager = _context.ManagerFactory.CreateExternalDbManager();
            var participants = extManager.GetParticipantIds(id);

            var sessions = sessionManager.GetAllSessionsForStudy(id);
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