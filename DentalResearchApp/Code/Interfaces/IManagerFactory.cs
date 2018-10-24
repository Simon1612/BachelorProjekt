using DentalResearchApp.Code.Impl;

namespace DentalResearchApp.Code.Interfaces
{
    public interface IManagerFactory
    {
        ILinkManager CreateLinkManager();

        ISurveyManager CreateSurveyManager();

        IUserManager CreateUserManager();
    }
}