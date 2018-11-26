using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;

namespace IntegrationTests.Helpers
{
    public interface ITestFixture
    {
        IHttpClientWrapper HttpClient { get; }
        IManagerFactory ManagerFactory { get; set; }

        void Dispose();
        void DropDatabases();
        Task SeedWithUsers();
        Task SignInAsAdmin();
        Task SignInAsResearcher();
        Task SignInAsVolunteer(string surveyName, string linkId, string participantEmail, int participantId);
        UserModel GetResearcherUserModel();
        UserModel GetAdminUserModel();
    }
}