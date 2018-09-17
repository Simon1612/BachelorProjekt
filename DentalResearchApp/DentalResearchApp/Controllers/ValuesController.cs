using System.Collections.Generic;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet, Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {

            var test = new SurveyContext();

            return new [] { "We're", "in!"};
        }
    }
}
