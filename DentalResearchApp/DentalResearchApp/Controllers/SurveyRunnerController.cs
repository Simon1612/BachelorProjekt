using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class SurveyRunnerController : Controller
    {
        // GET
        public IActionResult SurveyRunner()
        {
            return View();
        }

        [HttpGet("getSurvey"), Authorize]
        public async Task<string> GetSurvey(string surveyId)
        {
            var manager = new SurveyManager();

            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }
    }
}