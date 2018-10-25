using System.Collections.Generic;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class StudyController : Controller
    {
        [HttpGet]
        public ActionResult AllStudies()
        {
            var allStudiesModel = new AllStudiesViewModel()
            {
                AllStudyNames = new List<string>()
            };

            return View(allStudiesModel);
        }
    }
}