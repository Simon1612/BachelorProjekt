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


            //var studyDetails = new StudyModel()
            //{
            //    StudyName = "A study of My Little Pony and their dental hygiene",
            //    StudyDescription =
            //        "Never thought i would spent that much time inside a horses mouth when i studied as a dentist",
            //    Sessions = sessions,
            //    Patients = new List<PatientModel>()
            //};
            //Todo: Populer modellen med de rigtige study Data
            return View(viewModel);
        }

        ////Todo: Find ud af om dette skal bruges
        //[HttpGet("EditStudy")]
        //public ActionResult EditStudy()
        //{

        //    var study = new StudyViewModel()
        //    {
        //        StudyName = "A study of My Little Pony and their dental hygiene",
        //        StudyDescription =
        //            "Never thought i would spent that much time inside a horses mouth when i studied as a dentist",
        //        SurveysList = new SelectList(new List<Survey>()),
        //        PatientsList = new SelectList(new List<PatientModel>())
        //    };
        //    //Todo: Populer modellen med de rigtige study Data

        //    return View(study);
        //}
    }
}