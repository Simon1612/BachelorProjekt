using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize(Roles = "Administrator, Researcher")]
    [ApiController]
    public class SurveyController : Controller
    {
        private readonly IContext _context;

        public SurveyController(IContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("results")]
        public ActionResult Results()
        {
            return View();
        }
        
        [HttpGet("SurveyResults")]
        public async Task<ActionResult> SurveyResults()
        {
            var surveyResultsViewModel = new SurveyResultsViewModel
            {
                AllSurveyNames = await GetAllSurveyNames()
            };

            return View(surveyResultsViewModel);
        }

        public Task<List<string>> GetAllSurveyNames()
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            return Task.Run(() => manager.GetAllNames());
        }

        [HttpGet("getActive")]
        public async Task<JsonResult> GetActiveAsync()
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            var surveys = await manager.GetAllSurveys();

            return Json(surveys);
        }

        [HttpGet("getSurvey")]
        public async Task<string> GetSurvey(string surveyId)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }

        [HttpGet("create")]
        public async Task<JsonResult> Create(string name)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            await manager.CreateSurvey(name);

            return Json("Ok");
        }


        [HttpGet("delete")]
        public async Task<JsonResult> Delete(string id)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            await manager.DeleteSurvey(id);

            return Json("Ok");
        }

        [HttpGet("getResults")]
        public async Task<JsonResult> GetResults(string postId)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            var survey = await manager.GetResults(postId);

            return Json(survey);
        }
    }
}