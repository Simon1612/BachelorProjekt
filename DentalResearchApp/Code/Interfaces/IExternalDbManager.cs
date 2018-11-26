using System.Collections.Generic;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface IExternalDbManager
    {
        List<ParticipantInfo> GetParticipantInfo(int studyId);
        List<StudyListModel> GetAllStudyListModels();
        List<int> GetParticipantIds(int studyId);
        Study GetStudy(int studyId);
        List<string> GetParticipantEmails(int studyId);
    }
}