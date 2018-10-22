using DentalResearchApp.Code.Impl;
using DentalResearchApp.Code.Interfaces;

namespace DentalResearchApp.Models.Context
{
    public interface IContext
    {
        IManagerFactory ManagerFactory { get; set; }
        //IConfig Config { get; set; }
    }

    public class Context : IContext
    {
        public IManagerFactory ManagerFactory { get; set; }

        public Context(string connectionString)
        {
            ManagerFactory = new ManagerFactory(connectionString);
        }
    }
}