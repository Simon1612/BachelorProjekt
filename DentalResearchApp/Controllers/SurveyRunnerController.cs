using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize]
    public class SurveyRunnerController : Controller
    {
        [Authorize]
        public IActionResult SurveyRunner()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet("index")]
        public IActionResult SurveyRunnerIndex()
        {
            return View();
        }


        [HttpGet("getSurvey")]
        public async Task<string> GetSurvey(string surveyId)
        {
            var manager = new SurveyManager();

            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }


        [HttpPost("post")]
        public async Task<JsonResult> PostResult([FromBody]PostSurveyResultModel model)
        {
            var manager = new SurveyManager();

            //Get participantId from cookie!

            var id = Guid.NewGuid().ToString();

            var result = new SurveyResult
            {
                SurveyName = model.PostId,
                JsonResult = model.SurveyResult,
                ParticipantId = id,
                TimeStamp = DateTime.Now
            };

            await manager.SaveSurveyResult(result);

            return Json("Ok");
        }
    }
}