using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NSubstitute;
using Xunit;

namespace UnitTests
{
    public class UserManagerTests
    {
        private readonly IMongoClient _mockClient;
        private readonly IMongoDatabase _mockDb;
        private readonly IMongoCollection<UserModel> _mockUserCollection;
        private readonly IMongoCollection<UserCredentials> _mockCredsCollection;

        private readonly IMongoQueryable<Survey> _mockQueryable;

        public UserManagerTests()
        {
            _mockClient = Substitute.For<IMongoClient>();
            _mockDb = Substitute.For<IMongoDatabase>();
            _mockQueryable = Substitute.For<IMongoQueryable<Survey>>();
            _mockUserCollection = Substitute.For<IMongoCollection<UserModel>>();
            _mockCredsCollection = Substitute.For<IMongoCollection<UserCredentials>>();
        }

        [Fact]
        public async Task UserManager_CreateUser_CredsAndUserInsertedAsync()
        {
            _mockClient.GetDatabase(Arg.Any<string>()).Returns(_mockDb);
            _mockDb.GetCollection<UserModel>(Arg.Is("user_collection")).Returns(_mockUserCollection);
            _mockDb.GetCollection<UserCredentials>(Arg.Is("credentials_collection")).Returns(_mockCredsCollection);


            var manager = new UserManager(_mockClient, "test");

            var user = new UserModel(){Email = "Test"};
            var creds = new UserCredentials(){UserName = "Test"};

            await manager.CreateUser(user, creds);

            await _mockUserCollection.Received(1).InsertOneAsync(user);
            await _mockCredsCollection.Received(1).InsertOneAsync(creds);
        }
    }
}