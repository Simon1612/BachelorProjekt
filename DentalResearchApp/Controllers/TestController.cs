using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : Controller
    {
        private IConfig _config;
        public TestController(IConfig config)
        {
            _config = config;
        }
        [HttpGet]
        public string Get()
        {
            return "Ayy";
        }
    }
}