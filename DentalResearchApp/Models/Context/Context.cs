using DentalResearchApp.Code.Impl;
using DentalResearchApp.Code.Interfaces;

namespace DentalResearchApp.Models.Context
{
    public class Context : IContext
    {
        public IManagerFactory ManagerFactory { get; set; }

        public Context(string connectionString, string linkDbName, string surveyDbName, string userDbName)
        {
            ManagerFactory = new ManagerFactory(connectionString, linkDbName, surveyDbName, userDbName);
        }
    }
}