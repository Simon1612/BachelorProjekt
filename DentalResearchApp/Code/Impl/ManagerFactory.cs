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


        public ManagerFactory(string connectionString, string linkDbName, string surveyDbName, string userDbName)
        {
            _client = new MongoClient(connectionString);
            _linkDbName = linkDbName;
            _surveyDbName = surveyDbName;
            _userDbName = userDbName;
        }

        public ISurveyLinkManager CreateSurveyLinkManager()
        {
            return new SurveyLinkManager(_client, _linkDbName);
        }

        public ISurveyLinkManager CreateSignupLinkManager()
        {
            return new SurveyLinkManager(_client, _linkDbName);
        }

        public ISurveyManager CreateSurveyManager()
        {
            return new SurveyManager(_client, _surveyDbName);
        }

        public IUserManager CreateUserManager()
        {
            return new UserManager(_client, _userDbName);
        }
    }
}
