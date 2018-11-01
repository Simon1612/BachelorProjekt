using System.Collections.Generic;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class PatientController : Controller
    {

        public ActionResult AllPatients()
        {
            var allPatientsModel = new AllPatientsViewModel()
            {
                AllPatientNames = new List<string>()
            };

            return View(allPatientsModel);
        }
    }
}