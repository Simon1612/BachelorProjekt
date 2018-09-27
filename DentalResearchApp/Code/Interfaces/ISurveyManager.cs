using System.Collections.Generic;
using System.Threading.Tasks;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ISurveyManager
    {
        Task<Dictionary<string, string>> GetSurvey(string surveyName);
        Task<Dictionary<string, string>> GetAllSurveys();
        Task DeleteSurvey(string surveyName);
    }
}