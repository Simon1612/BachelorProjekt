using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface ISignupLinkManager
    {
        Task DeleteLink(string linkId);
        Task<SignupLinkModel> GetLink(string linkId);
        Task SendLink(string recipiantEmail, string baseUrl);
    }
}