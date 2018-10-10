using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    [Route("[controller]"), Authorize(Roles = nameof(Role.Volunteer))]
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
        public async Task<ActionResult<string>> GetSurvey(string surveyId)
        {
            var surveyNameFromCookie = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            if (surveyNameFromCookie != surveyId) //Participant is trying to change the name of survey in url?
                return BadRequest();

            var manager = new SurveyManager();
            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }


        [HttpPost("post")]
        public async Task<JsonResult> PostResult([FromBody]PostSurveyResultModel model)
        {
            //Get ids from cookie!
            var participantId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var linkId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Uri)?.Value;

            var result = new SurveyResult
            {
                SurveyName = model.PostId,
                JsonResult = model.SurveyResult,
                ParticipantId = participantId,
                TimeStamp = DateTime.Now,
            };

            //Save result to DB
            var surveyManager = new SurveyManager();
            await surveyManager.SaveSurveyResult(result);

            //This kills the cookie
            await HttpContext.SignOutAsync();
            
            //Delete link from DB
            var linkManager = new LinkManager();
            await linkManager.DeleteSurveyLink(linkId);


            return Json("Ok");
        }
    }
}