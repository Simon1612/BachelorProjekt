using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize(Roles = "Administrator, Researcher")]
    public class EditorController : Controller
    {
        private IContext _context;

        public EditorController(IContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Editor()
        {
            return View();
        }

        [HttpGet("getSurvey")]
        public async Task<string> GetSurvey(string surveyId)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();

            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }

        [HttpPost("changeJson")]
        public async Task<string> ChangeJson([FromBody]ChangeSurveyModel model)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();

            await manager.ChangeSurvey(model);

            return ""; //wat
        }

        [HttpGet("changeName")]
        public async Task<JsonResult> ChangeName(string id, string name)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();

            await manager.ChangeSurveyName(id, name);

            return Json("Ok");
        }
    }
}