using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var surveyManager = new SurveyManager();

            //Get participantId from cookie!
            var id = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = new SurveyResult
            {
                SurveyName = model.PostId,
                JsonResult = model.SurveyResult,
                ParticipantId = id,
                TimeStamp = DateTime.Now
            };

            await surveyManager.SaveSurveyResult(result);

            //Remove auth cookie
            await HttpContext.SignOutAsync();
            
            //Delete linkmodel from DB
            var linkManager = new LinkManager();
            //linkManager.DeleteSurveyLink() //Need linkId from somewhere.. maybe put in cookie?
            

            return Json("Ok");
        }
    }
}