namespace DentalResearchApp.Code.Interfaces
{
    public interface IManagerFactory
    {
        ISurveyLinkManager CreateSurveyLinkManager();

        ISurveyManager CreateSurveyManager();

        IUserManager CreateUserManager();
    }
}