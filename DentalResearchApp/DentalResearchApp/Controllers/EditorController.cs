using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class EditorController : Controller
    {
        [HttpGet]
        public IActionResult Editor()
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

        [HttpPost("changeJson"), Authorize]
        public async Task<string> ChangeJson([FromBody]ChangeSurveyModel model)
        {
            var manager = new SurveyManager();

            await manager.ChangeSurvey(model);

            return ""; //wat
        }

        [HttpGet("changeName"), Authorize]
        public async Task<JsonResult> ChangeName(string id, string name)
        {
            var manager = new SurveyManager();

            await manager.ChangeSurveyName(id, name);

            return Json("Ok");
        }
    }
}