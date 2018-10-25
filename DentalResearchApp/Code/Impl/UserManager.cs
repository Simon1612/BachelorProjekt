using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DentalResearchApp.Code.Impl
{
    public class UserManager : IUserManager
    {
        private readonly IMongoDatabase _db;

        public UserManager(IMongoClient client, string databaseName)
        {
            _db = client.GetDatabase(databaseName);
        }

        public async Task<UserModel> Authenticate(LoginModel login)
        {
            UserModel user = null;

            var credsColl = _db.GetCollection<UserCredentials>("credentials_collection");
            var storedUserCreds = await credsColl.AsQueryable().FirstOrDefaultAsync(x => x.UserName == login.Username);
            
            var hash = Hash.Create(login.Password, storedUserCreds.Salt);

            if (hash == storedUserCreds.Hash)
            {
                user = await GetUserModel(login.Username);
            }

            return user;
        }

        private async Task<UserModel> GetUserModel(string eMail)
        {
            var userColl = _db.GetCollection<UserModel>("user_collection");

            return await userColl.AsQueryable().FirstOrDefaultAsync(x => x.Email == eMail);
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var userColl = _db.GetCollection<UserModel>("user_collection");

            return await userColl.AsQueryable().ToListAsync();
        }

        public async Task CreateUser(UserModel userModel, UserCredentials userCreds)
        {
            var userColl = _db.GetCollection<UserModel>("user_collection");
            var credsColl = _db.GetCollection<UserCredentials>("credentials_collection");

            await userColl.InsertOneAsync(userModel);
            await credsColl.InsertOneAsync(userCreds);
        }

        private async void SeedWithDefaultUsers()
        {
            var user = new UserModel()
            {
                Email = "asdf@gmail.com",
                FirstName = "Super",
                LastName = "Mario",
                Role = Role.Administrator,
            };

            var password = "secret";

            var salt = Salt.Create();
            var hash = Hash.Create(password, salt);

            var login = new UserCredentials() { UserName = user.Email, Hash = hash, Salt = salt};

            await CreateUser(user, login);
        }
    }
}
