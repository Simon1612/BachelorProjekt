using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface IUserManager
    {
        Task<UserModel> Authenticate(LoginModel login);

        Task CreateUser(UserModel userModel, UserCredentials userCreds);
    }
}