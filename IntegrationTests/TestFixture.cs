using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DentalResearchApp;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Xml;
using Mongo2Go;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationTests
{
    public class TestFixture : IDisposable
    {
        private MongoDbRunner _dbRunner;
        private readonly TestServer _testServer;
        private readonly IMongoClient _mongoClient;

        public IHttpClientWrapper HttpClient { get; }

        public IManagerFactory ManagerFactory { get; set; }


        private readonly string _userDbName;
        private readonly string _surveyDbName;
        private readonly string _linkDbName;


        public TestFixture()
        {
            // Start local database
            var connString = StartMongoServer();

            // Create config
            _userDbName = "testUserDb";
            _surveyDbName = "testSurveyDb";
            _linkDbName = "testLinkDb";


            var dictionary = new Dictionary<string, string>
            {
                ["UserDbName"] = _userDbName,
                ["SurveyDbName"] = _surveyDbName,
                ["LinkDbName"] = _linkDbName,
                ["ConnectionString"] = connString
            };


            //Host + app setup
            var builder = new WebHostBuilder()
                .UseEnvironment("IntegrationTest")
                .ConfigureAppConfiguration((hostingContext, config) =>
                    //config.AddJsonFile("appsettings.IntegrationTest.json", optional: false, reloadOnChange: true))
                    config.AddInMemoryCollection(dictionary))
                .UseStartup<Startup>();


            _testServer = new TestServer(builder);
            _mongoClient = new MongoClient(connString);
            ManagerFactory = new ManagerFactory(_mongoClient, _linkDbName, _surveyDbName, _userDbName);
            HttpClient = new HttpClientWrapper(_testServer.CreateClient());
        }


        public void DropDatabases()
        {
            var databaseNames = new List<string>() { _userDbName, _surveyDbName, _linkDbName };

            Parallel.ForEach(databaseNames, name => { _mongoClient.DropDatabaseAsync(name); });
        }

        private async Task SignIn(string email, string password)
        {
            var login = new LoginModel {Username = email ,Password = password};

            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await HttpClient.PostAsync("cookie/login", content);
            var message = await response.Content.ReadAsStringAsync();

            if (!message.Contains("Success"))
                throw new UnauthorizedAccessException("Unable to login with user");
        }


        public async Task SignInAsResearcher()
        {
            await SignIn("researcher@test.test", "researcher");
        }

        public async Task SignInAsAdmin()
        {
            await SignIn("admin@test.test", "admin");
        }

        private string StartMongoServer()
        {
            _dbRunner = MongoDbRunner.Start(); 

            return _dbRunner.ConnectionString;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            _testServer.Dispose();
            _dbRunner.Dispose();
        }

        public async Task SeedWithUsers()
        {
            var userManager = ManagerFactory.CreateUserManager();

            var admin = new UserModel()
            {
                Email = "admin@test.test",
                FirstName = "admin",
                LastName = "admin",
                Role = Role.Administrator,
            };
            var adminPassword = "admin";
            var adminSalt = Salt.Create();
            var adminHash = Hash.Create(adminPassword, adminSalt);

            var adminCredentials = new UserCredentials() { UserName = admin.Email, Hash = adminHash, Salt = adminSalt };


            var researcher = new UserModel()
            {
                Email = "researcher@test.test",
                FirstName = "researcher",
                LastName = "researcher",
                Role = Role.Researcher,
            };
            var researcherPassword = "researcher";
            var researcherSalt = Salt.Create();
            var researcherHash = Hash.Create(researcherPassword, researcherSalt);

            var researcherCredentials = new UserCredentials() { UserName = researcher.Email, Hash = researcherHash, Salt = researcherSalt };


            await userManager.CreateUser(researcher, researcherCredentials);
            await userManager.CreateUser(admin, adminCredentials);
        }
    }
}
