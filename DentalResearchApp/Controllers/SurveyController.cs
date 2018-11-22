using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using DentalResearchApp.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace DentalResearchApp.Controllers
{
    [Route("[controller]"), Authorize(Roles = "Administrator, Researcher")]
    [ApiController]
    public class SurveyController : Controller
    {
        private readonly IContext _context;

        public SurveyController(IContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("results")]
        public ActionResult Results()
        {
            return View();
        }

        [HttpGet("SurveyResults")]
        public async Task<ActionResult> SurveyResults()
        {
            var surveyResultsViewModel = new SurveyResultsViewModel
            {
                AllSurveyNames = await GetAllSurveyNames()
            };

            return View(surveyResultsViewModel);
        }

        public Task<List<string>> GetAllSurveyNames()
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            return Task.Run(() => manager.GetAllNames());
        }

        [HttpGet("getActive")]
        public async Task<JsonResult> GetActiveAsync()
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            var surveys = await manager.GetAllSurveys();

            return Json(surveys);
        }

        [HttpGet("getSurvey")]
        public async Task<string> GetSurvey(string surveyId)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            var survey = await manager.GetSurvey(surveyId);

            return survey[surveyId];
        }

        [HttpGet("create")]
        public async Task<JsonResult> Create(string name)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            await manager.CreateSurvey(name);

            return Json("Ok");
        }


        [HttpGet("delete")]
        public async Task<JsonResult> Delete(string id)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            await manager.DeleteSurvey(id);

            return Json("Ok");
        }

        [HttpGet("getResults")]
        public async Task<JsonResult> GetResults(string postId)
        {
            var manager = _context.ManagerFactory.CreateSurveyManager();
            var survey = await manager.GetResults(postId);

            return Json(survey);
        }

        [HttpGet("FindResults")]
        public IActionResult FindResults()
        {
            var externalManager = _context.ManagerFactory.CreateExternalDbManager();

            var findResultsViewModel = new FindResultsViewModel()
            {
                Studies = externalManager.GetAllStudyListModels().Select(x => x.StudyName).ToList()
            };

            return View(findResultsViewModel);
        }

        [HttpPost("FindResultsSessions")]
        public IActionResult FindResultSessions([FromForm]FindResultsViewModel findResultsViewModel)
        {
            var externalManager = _context.ManagerFactory.CreateExternalDbManager();
            var sessionManager = _context.ManagerFactory.CreateSessionManager();

            var selectedStudy = externalManager
                .GetAllStudyListModels().SingleOrDefault(x => x.StudyName == findResultsViewModel.SelectedStudy);

            var findResultsSessionsModel = new FindResultsViewModel();
            if (selectedStudy != null)
            {
                findResultsSessionsModel.StudyId = selectedStudy.StudyId;
                findResultsSessionsModel.SelectedStudy = selectedStudy.StudyName;
                findResultsSessionsModel.Sessions = sessionManager
                    .GetAllSessionModelsForStudy(selectedStudy.StudyId)
                    .Select(x => x.SessionName).ToList();
                findResultsSessionsModel.Studies =
                    externalManager.GetAllStudyListModels().Select(x => x.StudyName).ToList();
                findResultsSessionsModel.Participants = externalManager.GetParticipantIds(selectedStudy.StudyId);
            }

            return View("FindResults", findResultsSessionsModel);
        }

        [HttpPost("FindResultsParticipants")]
        public async Task<IActionResult> FindResultsParticipants([FromForm]FindResultsViewModel findResultsViewModel)
        {
            var externalManager = _context.ManagerFactory.CreateExternalDbManager();
            var sessionManager = _context.ManagerFactory.CreateSessionManager();

            var selectedSession = await sessionManager.GetStudySession(findResultsViewModel.StudyId, findResultsViewModel.SelectedSession);
            var findResultsSurveysViewModel = new FindResultsViewModel();
            if (selectedSession != null)
            {
                findResultsSurveysViewModel.StudyId = findResultsViewModel.StudyId;
                findResultsSurveysViewModel.SelectedStudy = findResultsViewModel.SelectedStudy;

                findResultsSurveysViewModel.Sessions = sessionManager
                    .GetAllSessionModelsForStudy(findResultsViewModel.StudyId)
                    .Select(x => x.SessionName).ToList();

                findResultsSurveysViewModel.Studies =
                    externalManager.GetAllStudyListModels().Select(x => x.StudyName).ToList();

                findResultsSurveysViewModel.Participants = externalManager.GetParticipantIds(findResultsSurveysViewModel.StudyId);

                findResultsSurveysViewModel.SessionId = selectedSession.Id;
                findResultsSurveysViewModel.SelectedSession = selectedSession.SessionName;

                findResultsSurveysViewModel.Surveys = selectedSession.Surveys;
            }

            return View("FindResults", findResultsSurveysViewModel);
        }

        [HttpPost("FindResultsSurveys")]
        public async Task<IActionResult> FindResultSurveys([FromForm]FindResultsViewModel findResultsViewModel)
        {
            var externalManager = _context.ManagerFactory.CreateExternalDbManager();
            var sessionManager = _context.ManagerFactory.CreateSessionManager();

            var selectedSession = await sessionManager.GetStudySession(findResultsViewModel.StudyId, findResultsViewModel.SelectedSession);
            var findResultsSurveysViewModel = new FindResultsViewModel();
            if (selectedSession != null)
            {
                findResultsSurveysViewModel.StudyId = findResultsViewModel.StudyId;
                findResultsSurveysViewModel.SelectedStudy = findResultsViewModel.SelectedStudy;

                findResultsSurveysViewModel.Sessions = sessionManager
                    .GetAllSessionModelsForStudy(findResultsViewModel.StudyId)
                    .Select(x => x.SessionName).ToList();

                findResultsSurveysViewModel.Studies =
                    externalManager.GetAllStudyListModels().Select(x => x.StudyName).ToList();

                findResultsSurveysViewModel.Participants = externalManager.GetParticipantIds(findResultsViewModel.StudyId);

                findResultsSurveysViewModel.SessionId = selectedSession.Id;
                findResultsSurveysViewModel.SelectedSession = selectedSession.SessionName;
                findResultsSurveysViewModel.Surveys = selectedSession.Surveys;
                findResultsSurveysViewModel.SelectedSurvey = findResultsViewModel.SelectedSurvey;
            }

            return View("FindResults", findResultsSurveysViewModel);
        }

        [HttpPost("ShowFindResults")]
        public async Task<IActionResult> ShowFindResults([FromForm] FindResultsViewModel findResultsViewModel)
        {
            var externalManager = _context.ManagerFactory.CreateExternalDbManager();
            var sessionManager = _context.ManagerFactory.CreateSessionManager();

            var selectedSession = await sessionManager.GetStudySession(findResultsViewModel.StudyId, findResultsViewModel.SelectedSession);

            var findResultsFinalViewModel = new FindResultsViewModel()
            {
                Studies = externalManager.GetAllStudyListModels().Select(x => x.StudyName).ToList(),
                SelectedSession = selectedSession.SessionName,
                StudyId = findResultsViewModel.StudyId,
                SelectedStudy = findResultsViewModel.SelectedStudy,
                Surveys = selectedSession.Surveys,
                SelectedSurvey = findResultsViewModel.SelectedSurvey,
                Sessions = sessionManager
                    .GetAllSessionModelsForStudy(findResultsViewModel.StudyId)
                    .Select(x => x.SessionName).ToList(),
                SessionId = selectedSession.Id
            };

            return View("FindResults", findResultsFinalViewModel);
        }
    }
}