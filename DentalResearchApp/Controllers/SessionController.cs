﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class SessionController : Controller
    {
        private readonly IContext _context;

        public SessionController(IContext context)
        {
            _context = context;
        }

        [HttpPost("StartSession")]
        public async Task<IActionResult> StartSession(int studyId, string sessionName)
        {
            var sessionManager = _context.ManagerFactory.CreateSessionManager();
            var externalDbManager = _context.ManagerFactory.CreateExternalDbManager();
            var surveyLinkManager = _context.ManagerFactory.CreateSurveyLinkManager();

            var studySessionModel = sessionManager.GetStudySession(studyId, sessionName).Result;

            var id = studySessionModel.Id;

            var userSessions = await sessionManager.GetAllUserSessionsForStudySession(id);
            var participants = externalDbManager.GetParticipantInfo(studyId);

            foreach (var survey in studySessionModel.Surveys)
            {

                foreach (var session in userSessions)
                {
                    //Create surveylink wih a reference to UserSessions ObjectId

                    var participantId = session.ParticipantId;
                    var participantInfo = participants.FirstOrDefault(x => x.ParticipantId == participantId);

                    var baseUrl = BaseUrlHelper.GetBaseUrl(Request);

                    var link = new SurveyLinkModel
                    {
                        ParticipantId = participantInfo.ParticipantId,
                        ParticipantEmail = participantInfo.Email,
                        SurveyName = survey,
                        UserSessionId = session.Id
                    };

                    await surveyLinkManager.SendLink(link, baseUrl);
                }
            }

            await sessionManager.SetStudySessionStarted(studyId, sessionName);
            return RedirectToAction("AllStudies", "Study");
        }


        [HttpGet("SessionDetails")]
        public IActionResult SessionDetails(int studyId, string sessionName, string studyName)
        {
            var manager = _context.ManagerFactory.CreateSessionManager();
            var sessionModel = manager.GetStudySession(studyId, sessionName).Result;


            var sessionViewModel = new StudySessionViewModel()
            {
                SessionName = sessionModel.SessionName,
                StudyId = sessionModel.StudyId,
                AllSurveys = GetAllSurveyNamesList(),
                SelectedSurveys = sessionModel.Surveys,
                IsStarted = sessionModel.IsStarted
            };

            ViewBag.studyName = studyName;

            return View(sessionViewModel);
        }

        [HttpGet("CreateStudySession")]
        public IActionResult CreateSession(int studyId, string studyName)
        {
            var sessionModel = new StudySessionViewModel { StudyId = studyId, AllSurveys = GetAllSurveyNamesList() };
            ViewBag.studyName = studyName;

            return View(sessionModel);
        }

        [HttpPost("CreateStudySession")]
        public async Task<IActionResult> CreateSession(StudySessionViewModel sessionModel)
        {
            var sessionManager = _context.ManagerFactory.CreateSessionManager();

            var studySessionModel = new StudySessionModel()
            {
                SessionName = sessionModel.SessionName,
                StudyId = sessionModel.StudyId,
                Surveys = sessionModel.SelectedSurveys.ToList(),
                IsStarted = false
            };

            await sessionManager.CreateStudySession(studySessionModel);

            var studyManager = _context.ManagerFactory.CreateExternalDbManager();

            var userIdList = studyManager.GetParticipantIds(sessionModel.StudyId);
            var model = await sessionManager.GetStudySession(studySessionModel.StudyId, studySessionModel.SessionName);

            Parallel.ForEach(userIdList, x => sessionManager.CreateUserSession(new UserSession()
            {
                ParticipantId = x,
                StudySessionId = model.Id
            }));

            return RedirectToAction("AllStudies", "Study");
        }


        [HttpPost("DeleteSession")]
        public IActionResult DeleteSession(string sessionName, int studyId)
        {

            var sessionManager = _context.ManagerFactory.CreateSessionManager();

            var studySession = sessionManager.GetStudySession(studyId, sessionName).Result;

            sessionManager.DeleteSession(studySession);
            return RedirectToAction("AllStudies", "Study");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public List<SelectListItem> GetAllSurveyNamesList()
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            var allSurveys = manager.GetAllNames()
                .Result.Select(x => new SelectListItem
                {
                    Text = x.ToString(),
                    Value = x.ToString(),
                    Selected = false
                })
                .ToList();

            return allSurveys;
        }


    }
}