using DentalResearchApp.Code.Impl;
using DentalResearchApp.Code.Interfaces;
using MongoDB.Driver;

namespace DentalResearchApp.Models.Context
{
    public class Context : IContext
    {
        public IManagerFactory ManagerFactory { get; set; }

        public Context(IMongoClient client, string linkDbName, string surveyDbName, string userDbName)
        {
            ManagerFactory = new ManagerFactory(client, linkDbName, surveyDbName, userDbName);
        }
    }
}