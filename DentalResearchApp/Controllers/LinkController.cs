using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize(Roles = "Administrator, Researcher")]
    public class LinkController : Controller
    {
        [HttpPost("sendSurveyLink")]
        public async Task<JsonResult> SendSurveyLink([FromBody] SendSurveyLinkModel model)
        {
            var host = Request.Host.Host;

            if (host == "localhost")
                host += ":" + Request.Host.Port;

            var baseUrl = "https://" + host;

            var manager = new LinkManager();

            await manager.SendSurveyLink(model.SurveyName, model.ParticipantId, baseUrl);

            return Json("Ok");
        }
    }
}