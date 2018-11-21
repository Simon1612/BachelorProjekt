using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Models;

namespace DentalResearchApp.Code.Interfaces
{
    public interface IUserManager
    {
        Task<UserModel> Authenticate(LoginModel login);
        Task<List<UserModel>> GetAllUsers();
        Task CreateUser(UserModel userModel, UserCredentials userCreds);
        Task DeleteUser(UserModel userModel, UserCredentials userCreds);
        Task<UserModel> GetUserModel(string eMail);
        Task UpdateUserCredentials(UserCredentials model);
        Task<UserCredentials> GetUserCredentials(string email);
        Task UpdateUserData(UserModel userModel);
    }
}