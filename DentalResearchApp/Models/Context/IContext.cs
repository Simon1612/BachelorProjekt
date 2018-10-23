using DentalResearchApp.Code.Interfaces;

namespace DentalResearchApp.Models.Context
{
    public interface IContext
    {
        IManagerFactory ManagerFactory { get; set; }
        //IConfig Config { get; set; }
    }
}