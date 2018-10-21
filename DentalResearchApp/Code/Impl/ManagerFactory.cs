using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using MongoDB.Driver;

namespace DentalResearchApp.Code.Impl
{
    public class ManagerFactory : IManagerFactory
    {
        private readonly string _connectionString;

        public ManagerFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ILinkManager CreateLinkManager()
        {
            var client = new MongoClient(_connectionString);

            return new LinkManager(client);
        }

        public ISurveyManager CreateSurveyManager()
        {
            var client = new MongoClient(_connectionString);

            return new SurveyManager(client);
        }

        public IUserManager CreateUserManager()
        {
            var client = new MongoClient(_connectionString);

            return new UserManager(client);
        }
    }
}
