using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize]
    public class SurveyRunnerController : Controller
    {
        // GET
        public IActionResult SurveyRunner()
        {
            return
            View();
        }

        [HttpGet("getSurvey")]
        public async Task<string> GetSurvey(string surveyId)
        {
            var manager = new SurveyManager();

            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }

        [HttpPost("post")]
        public JsonResult PostResult([FromBody]PostSurveyResultModel model)
        {
            var db = new SessionStorage(HttpContext.Session);
            db.PostResults(model.PostId, model.SurveyResult);
            return Json("Ok");
        }
    }
}