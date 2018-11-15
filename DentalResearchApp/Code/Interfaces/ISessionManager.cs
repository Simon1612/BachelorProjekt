using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ISessionManager
    {
        Task CreateSession(StudySessionModel studySessionModel);
        Task DeleteSession(StudySessionModel studySessionModel);
        Task<StudySessionModel> GetStudySession(int studyId, string sessionName);
        List<string> GetAllSessionsForStudy(int studyId);
    }
}