using DentalResearchApp.Models;

namespace DentalResearchApp
{
    public interface IUserManager
    {
        UserModel Authenticate(LoginModel login);

        void CreateUser(UserModel userModel);
    }
}