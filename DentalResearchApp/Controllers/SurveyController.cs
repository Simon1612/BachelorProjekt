﻿using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class SurveyController : Controller
    {

        [HttpGet, Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("results"), Authorize]
        public ActionResult Results()
        {
            return View();
        }


        [HttpGet("getActive"), Authorize]
        public async Task<JsonResult> GetActiveAsync()
        {
            var manager = new SurveyManager();
            var surveys = await manager.GetAllSurveys();

            return Json(surveys);
        }

        [HttpGet("getSurvey"), Authorize]
        public async Task<string> GetSurvey(string surveyId)
        {
            var manager = new SurveyManager();
            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }

        [HttpGet("create"), Authorize]
        public JsonResult Create(string name)
        {
            var db = new SessionStorage(HttpContext.Session);
            db.StoreSurvey(name, "{}");
            return Json("Ok");
        }


        [HttpGet("delete"), Authorize]
        public async Task<JsonResult> Delete(string id)
        {
            await new SurveyManager().DeleteSurvey(id);

            return Json("Ok");
        }

        [HttpGet("getResults"), Authorize]
        public async Task<JsonResult> GetResults(string postId)
        {
            var manager = new SurveyManager();

            var survey = await manager.GetResults(postId);

            return Json(survey);
        }
    }
}