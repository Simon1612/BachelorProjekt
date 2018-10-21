using DentalResearchApp.Code.Impl;
using DentalResearchApp.Code.Interfaces;

namespace DentalResearchApp.Models.Context
{
    public interface IContext
    {
        IManagerFactory ManagerFactory { get; set; }
        //IConfig Config { get; set; }
    }

    public class TestContext : IContext
    {
        public IManagerFactory ManagerFactory { get; set; }

        public TestContext()
        {
            ManagerFactory = new ManagerFactory(new TestConfig().MongoConnectionString);
        }
    }


    public class ProdContext : IContext
    {
        public IManagerFactory ManagerFactory { get; set; }

        public ProdContext()
        {
            ManagerFactory = new ManagerFactory(new ProdConfig().MongoConnectionString);
        }
    }
}