using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize]
    public class SurveyRunnerController : Controller
    {
        private readonly IContext _context;

        public SurveyRunnerController(IContext context)
        {
            _context = context;
        }

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
            //Get users role
            var usersRoleString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role && x.Type != null).Value;
            Enum.TryParse(usersRoleString, out Role usersRole);

            //Only post results if users is a volunteer
            if (usersRole != Role.Administrator && usersRole != Role.Researcher)
            {
                var surveyNameFromCookie = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name && x.Type != null).Value;

                if (surveyNameFromCookie != surveyId) //Participant is trying to change the name of survey in url?
                    return BadRequest();
            }

            var manager = _context.ManagerFactory.CreateSurveyManager();
            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }


        [HttpPost("post")]
        public async Task<JsonResult> PostResult([FromBody]PostSurveyResultModel model)
        {
            //Get users role
            var usersRoleString = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role && x.Type != null).Value;
            Enum.TryParse(usersRoleString, out Role usersRole);

            //Only post results if users is a volunteer
            if (usersRole == Role.Volunteer)
            {
                //Get ids from cookie!
                var participantId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier && x.Type != null).Value;
                var linkId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Uri && x.Type != null).Value;

                var result = new SurveyResult
                {
                    SurveyName = model.PostId,
                    JsonResult = model.SurveyResult,
                    ParticipantId = participantId,
                    TimeStamp = DateTime.Now,
                };

                //Save result to DB
                var surveyManager = _context.ManagerFactory.CreateSurveyManager();
                await surveyManager.SaveSurveyResult(result);

                //This kills the cookie
                await HttpContext.SignOutAsync();

                //Delete link from DB
                var linkManager = _context.ManagerFactory.CreateSurveyLinkManager();
                await linkManager.DeleteLink(linkId);
            }

            return Json("Ok");
        }
    }
}
