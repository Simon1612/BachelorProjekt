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
            var allStudiesModel = new AllStudiesViewModel()
            {
                AllStudyNames = new List<string>() { "My Little first Study" }
            };
            //Todo: Populer modellen med de rigtige study names

            return View(allStudiesModel);
        }

        [HttpGet("StudyDetails")]
        public ActionResult StudyDetails(string id)
        {

            var studyDetails = new StudyModel()
            {
                StudyName = "A study of My Little Pony and their dental hygiene",
                StudyDescription =
                    "Never thought i would spent that much time inside a horses mouth when i studied as a dentist",
                Sessions = new List<SessionModel>(),
                Patients = new List<PatientModel>()
            };
            //Todo: Populer modellen med de rigtige study Data
            return View(studyDetails);
        }

        //Todo: Find ud af om dette skal bruges
        [HttpGet("EditStudy")]
        public ActionResult EditStudy()
        {

            var study = new StudyViewModel()
            {
                StudyName = "A study of My Little Pony and their dental hygiene",
                StudyDescription =
                    "Never thought i would spent that much time inside a horses mouth when i studied as a dentist",
                SurveysList = new SelectList(new List<Survey>()),
                PatientsList = new SelectList(new List<PatientModel>())
            };
            //Todo: Populer modellen med de rigtige study Data

            return View(study);
        }
    }
}