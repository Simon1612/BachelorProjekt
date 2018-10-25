using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ISurveyManager
    {
        Task<Dictionary<string, string>> GetSurvey(string surveyName);

        Task<Dictionary<string, string>> GetAllSurveys();

        Task DeleteSurvey(string surveyName);

        Task ChangeSurvey(ChangeSurveyModel model);

        Task ChangeSurveyName(string id, string name);

        Task SaveSurveyResult(SurveyResult result);

        Task<List<string>> GetResults(string postId);

        Task CreateSurvey(string surveyName);

        Task<List<string>> GetAllNames();
    }
}