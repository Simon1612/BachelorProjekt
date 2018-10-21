using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ILinkManager
    {
        Task DeleteSurveyLink(string linkId);
        Task<SurveyLinkModel> GetSurveyLink(string linkId);
        void SeedWithDefaultLinks();
        Task SendSurveyLink(string surveyName, string participantId, string baseUrl);
    }
}