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
        [HttpGet, Authorize(Roles = nameof(Role.Administrator))]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new [] { "We're", "in!"};
        }
    }
}
