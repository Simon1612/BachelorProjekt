using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{

    [Route("[controller]"), Authorize(Roles = "Administrator, Researcher")]
    public class LinkController : Controller
    {
        private readonly IContext _context;
        public LinkController(IContext context)
        {
            _context = context;
        }

        [HttpGet("/SendSurvey")]
        public IActionResult SendSurvey()
        {
            var sendSurveyModel = new SendSurveyModel();
            var studiesList = new List<string>();

            var patientsList = new List<string>();

            var manager = _context.ManagerFactory.CreateSurveyManager();
            var surveyList = new List<string>();

            foreach (var survey in manager.GetAllSurveys().Result)
            {
                surveyList.Add(survey.Key);
            }

            sendSurveyModel.Studies = studiesList;
            sendSurveyModel.Patients = patientsList;
            sendSurveyModel.Survey = surveyList;

            return View(sendSurveyModel);
        }

    
        [HttpPost("sendSurveyLink")]
        public async Task<JsonResult> SendSurveyLink([FromBody] SendSurveyLinkModel model)
        {
            var host = Request.Host.Host;

            if (host == "localhost")
                host += ":" + Request.Host.Port;

            var baseUrl = "https://" + host;

            var manager = _context.ManagerFactory.CreateSurveyLinkManager();

            await manager.SendLink(model.SurveyName, model.ParticipantEmail, model.ParticipantId, baseUrl);

            return Json("Ok");
        }

        public async Task<JsonResult> SendSignupLink([FromBody] string emailToInvite)
        {


            return Json("Ok");
        }
    }
}