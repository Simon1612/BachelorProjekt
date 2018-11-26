using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]")]
    public class UserSessionController : Controller
    {

        private readonly IContext _context;

        public UserSessionController(IContext context)
        {
            _context = context;
        }

        public IActionResult UserSessions(int participantId, int studyId)
        {
            var study = _context.ManagerFactory.CreateExternalDbManager().GetStudy(studyId);

            var sessionManager = _context.ManagerFactory.CreateSessionManager();
            var sessionNames = sessionManager.GetAllStudySessionsNamesForStudy(studyId);
            //var userSessions = await sessionManager.GetUserSessionsForStudy(studyId, participantId);


            var viewModel = new UserSessionsViewModel
            {
                ParticipantId = participantId,
                StudyName = study.StudyName,
                SessionNames = sessionNames,
                StudyId = studyId
            };


            return View(viewModel);
        }

        [HttpGet("UserSessionDetailsModal")]
        public async Task<IActionResult> UserSessionDetailsModal(int studyId, int participantId, string sessionName)
        {
            var manager = _context.ManagerFactory.CreateSessionManager();
            var studySession = await manager.GetStudySession(studyId, sessionName);

            var userSession = await manager.GetAllUserSessionsForStudySession(studySession.Id, participantId);



            return View();
        }
    }
}