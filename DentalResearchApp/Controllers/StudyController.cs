using System.Collections.Generic;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class StudyController : Controller
    {
        [HttpGet("AllStudies")]
        public ActionResult AllStudies()
        {
            var allStudiesModel = new AllStudiesViewModel()
            {
                AllStudyNames = new List<string>() {"My Little first Study" }
            };

            return View(allStudiesModel);
        }

        [HttpGet("StudyDetails")]
        public ActionResult StudyDetails(string id)
        {

            var studyDetails = new StudyModel()
            {
                StudyName = "An in-depth study of My Little Pony and their dental hygiene",
                StudyDescription =
                    "Never thought i would spent that much time inside a horses mouth when i studied as a dentist",
                Surveys = new List<Survey>(),
                Patients = new List<PatientModel>()
            };

            return View(studyDetails);
        }
    }
}