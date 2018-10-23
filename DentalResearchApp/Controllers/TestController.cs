﻿using System;
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
        private readonly IContext _context;
        public TestController(IContext context)
        {
            _context = context;
        }
        [HttpGet]
        public string Get()
        {

            var count = HttpContext.Request.Cookies.Count.ToString();

            return count;
        }
    }
}