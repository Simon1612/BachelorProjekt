using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Impl
{
    public interface ILinkManager
    {
        Task DeleteSurveyLink(string linkId);
        Task<SurveyLinkModel> GetSurveyLink(string linkId);
        void SeedWithDefaultLinks();
    }
}