using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ISurveyLinkManager
    {
        Task SaveSurveyLink(SurveyLinkModel surveyLink);
        Task DeleteLink(string linkId);
        Task<SurveyLinkModel> GetLink(string linkId);
        Task SendLink(string surveyName, string participantEmail, string participantId, string baseUrl);
    }
}