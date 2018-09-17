using System.Collections.Generic;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ISurveyManager
    {
        Dictionary<string, string> GetSurveyByName(string surveyName);
        Dictionary<string, string> GetAllSurveys();

    }
}