using DentalResearchApp.Code.Interfaces;
using MongoDB.Driver;

namespace DentalResearchApp.Code.Impl
{
    public class ManagerFactory : IManagerFactory 
    {
        private readonly IMongoClient _client;
        private readonly string _linkDbName;
        private readonly string _surveyDbName;
        private readonly string _userDbName;
        private readonly string _sessionDbName;


        public ManagerFactory(IMongoClient client, string linkDbName, string surveyDbName, string userDbName, string sessionDbName)
        {
            _client = client;
            _linkDbName = linkDbName;
            _surveyDbName = surveyDbName;
            _userDbName = userDbName;
            _sessionDbName = sessionDbName;
        }

        public ISurveyLinkManager CreateSurveyLinkManager()
        {
            return new SurveyLinkManager(_client, _linkDbName);
        }

        public ISignupLinkManager CreateSignupLinkManager()
        {
            return new SignupLinkManager(_client, _linkDbName);
        }

        public ISurveyManager CreateSurveyManager()
        {
            return new SurveyManager(_client, _surveyDbName);
        }

        public IUserManager CreateUserManager()
        {
            return new UserManager(_client, _userDbName);
        }

        public ISessionManager CreateSessionManager()
        {
            return new SessionManager(_client, _sessionDbName);
        }
    }
}
