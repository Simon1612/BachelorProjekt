using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ISessionManager
    {
        Task CreateSession(StudySessionModel studySessionModel);
        Task<StudySessionModel> GetStudySession(string studyId, string sessionName);
        List<string> GetAllSessionsForStudy(string studyId);
    }
}