using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
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


        [HttpGet("getActive"), Authorize]
        public async Task<JsonResult> GetActiveAsync()
        {
            var manager = new SurveyManager();
            var surveys = await manager.GetAllSurveys();

            return Json(surveys);
        }

        [HttpGet("getSurvey"), Authorize]
        public async Task<string> GetSurvey(string id)
        {
            var manager = new SurveyManager();
            var survey = await manager.GetSurvey(id);

            return survey[id];
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



        [HttpGet("results"), Authorize]
        public JsonResult GetResults(string postId)
        {
            var db = new SessionStorage(HttpContext.Session);
            return Json(db.GetResults(postId));
        }
    }
}