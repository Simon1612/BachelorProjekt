using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{

        [Route("api/[controller]")]
        [ApiController]
        public class SurveyController : Controller
        {

            [HttpGet, Authorize]
            public ActionResult Index()
            {
                return View();
            }


        [HttpGet("getActive"), Authorize]
            public JsonResult GetActive()
            {
                var context = new SurveyContext();
                var surveys = context.GetAllSurveys();
                
                return Json(surveys);
            }   

            [HttpGet("getSurvey"), Authorize]
            public string GetSurvey(string surveyId)
            {
                var context = new SurveyContext();
                var survey = context.GetSurveyByName(surveyId);

                return survey[surveyId];
            }

            [HttpGet("create"), Authorize]
            public JsonResult Create(string name)
            {
                var db = new SessionStorage(HttpContext.Session);
                db.StoreSurvey(name, "{}");
                return Json("Ok");
            }

            [HttpGet("changeName"), Authorize]
            public JsonResult ChangeName(string id, string name)
            {
                var db = new SessionStorage(HttpContext.Session);
                db.ChangeName(id, name);
                return Json("Ok");
            }

            [HttpPost("changeJson"), Authorize]
            public string ChangeJson([FromBody]ChangeSurveyModel model)
            {
                var db = new SessionStorage(HttpContext.Session);
                db.StoreSurvey(model.Id, model.Json);
                return db.GetSurvey(model.Id);
            }

            [HttpGet("delete"), Authorize]
            public JsonResult Delete(string id)
            {
                var db = new SessionStorage(HttpContext.Session);
                db.DeleteSurvey(id);
                return Json("Ok");
            }

            [HttpPost("post"), Authorize]
            public JsonResult PostResult([FromBody]PostSurveyResultModel model)
            {
                var db = new SessionStorage(HttpContext.Session);
                db.PostResults(model.PostId, model.SurveyResult);
                return Json("Ok");
            }

            [HttpGet("results"), Authorize]
            public JsonResult GetResults(string postId)
            {
                var db = new SessionStorage(HttpContext.Session);
                return Json(db.GetResults(postId));
            }
        }
}