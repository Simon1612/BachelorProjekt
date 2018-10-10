﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class LinkController : Controller
    {
        [HttpGet("/SendSurvey")]
        public IActionResult SendSurvey()
        {
            var sendSurveyModel = new SendSurveyModel();
            var studiesList = new List<string>() {"study 1", "study 2", "study 3", "study 4"};
            var patientsList = new List<string>()
            {
                "Bob",
                "Ole",
                "Jens",
                "Pia",
                "Henning",
                "Ham den underlige der altid sidder i hjørnet til møderne"
            };
            var manager = new SurveyManager();
            var surveyList = new List<string>();
            foreach (var survey in manager.GetAllSurveys().Result)
            {
                surveyList.Add(survey.Key);
            }

            sendSurveyModel.Studies = studiesList;
            sendSurveyModel.Patients = patientsList;
            sendSurveyModel.Survey = surveyList;

            return View(sendSurveyModel);
        }

        [HttpPost("CreateLink")]
        public JsonResult CreateLink(string surveyName, List<string> patientsList)
        {
            return Json("Ok");
        }
    }
}