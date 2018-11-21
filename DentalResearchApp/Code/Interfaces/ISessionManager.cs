using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using MongoDB.Bson;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ISessionManager
    {
        Task CreateStudySession(StudySessionModel studySessionModel);
        Task DeleteSession(StudySessionModel studySessionModel);
        Task<StudySessionModel> GetStudySession(int studyId, string sessionName);
        Task CreateUserSession(UserSession userSessionModel);
        List<string> GetAllStudySessionsNamesForStudy(int studyId);
        Task<List<UserSession>> GetAllUserSessionsForStudySession(ObjectId studySessionId);
    }
}