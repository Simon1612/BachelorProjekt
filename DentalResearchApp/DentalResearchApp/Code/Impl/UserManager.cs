using System;
using System.Linq;
using DentalResearchApp.Models;
using MongoDB.Driver;

namespace DentalResearchApp.Code.Impl
{
    public class UserManager : IUserManager
    {
        private readonly IMongoDatabase _db;

        public UserManager()
        {
            var client = new MongoClient("mongodb+srv://test:test@2018e21-surveydb-wtdmw.mongodb.net/test?retryWrites=true");
            _db = client.GetDatabase("UserDb");

            if (!_db.ListCollectionNames().Any())
                SeedWithDefaultUsers();
        }

        public UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            var credsColl = _db.GetCollection<UserCredentials>("credentials_collection");
            var storedUserCreds = credsColl.AsQueryable().FirstOrDefault(x => x.UserName == login.Username);

            var hash = Hash.Create(login.Password, storedUserCreds?.Salt);

            if (hash == storedUserCreds?.Hash)
            {
                user = GetUserModel(login.Username);
            }

            return user;
        }


        private UserModel GetUserModel(string userName)
        {
            var userColl = _db.GetCollection<UserModel>("user_collection");

            return userColl.AsQueryable().First(x => x.UserName == userName);
        }

       
        public async void CreateUser(UserModel userModel, UserCredentials userCreds)
        {
            var userColl = _db.GetCollection<UserModel>("user_collection");
            var credsColl = _db.GetCollection<UserCredentials>("credentials_collection");

            await userColl.InsertOneAsync(userModel);
            await credsColl.InsertOneAsync(userCreds);
        }

        private void SeedWithDefaultUsers()
        {
            var user = new UserModel()
            {
                Birthdate = DateTime.Now,
                Email = "asdf@gmail.com",
                FirstName = "Super",
                LastName = "Mario",
                Role = Role.Administrator,
                UserName = "mario"
            };


            var username = "mario";
            var password = "secret";

            var salt = Salt.Create();
            var hash = Hash.Create(password, salt);

            var login = new UserCredentials() { UserName = username, Hash = hash, Salt = salt};

            CreateUser(user, login);
        }
    }
}
