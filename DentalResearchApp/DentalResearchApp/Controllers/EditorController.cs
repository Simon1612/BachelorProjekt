using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;

namespace DentalResearchApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorController : Controller
    {
        [HttpGet]
        public IActionResult Editor()
        {
            return View();
        }

        [HttpGet("getSurvey")]
        public JsonResult GetSurvey(string surveyId)
        {
            return new JsonResult(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(@"C:\Users\Casper\Desktop\BachelorProjekt\repos\BachelorProjekt\DentalResearchApp\DentalResearchApp\Views\Survey\Json\ProductFeedbackSurvey.json"));
        }
    }
}