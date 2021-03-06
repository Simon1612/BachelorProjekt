﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DentalResearchApp;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Mongo2Go;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace IntegrationTests.Helpers
{
    public class TestFixture : IDisposable, ITestFixture
    {
        private MongoDbRunner _dbRunner;
        private readonly TestServer _testServer;
        private readonly IMongoClient _mongoClient;

        public IHttpClientWrapper HttpClient { get; }

        public IManagerFactory ManagerFactory { get; set; }


        private readonly string _userDbName;
        private readonly string _surveyDbName;
        private readonly string _linkDbName;
        private readonly string _sessionDbName;



        public TestFixture()
        {
            // Start local database
            var connString = StartMongoServer();

            // Create config
            _userDbName = "testUserDb";
            _surveyDbName = "testSurveyDb";
            _linkDbName = "testLinkDb";
            _sessionDbName = "testSessionDb";


            var dictionary = new Dictionary<string, string>
            {
                ["UserDbName"] = _userDbName,
                ["SurveyDbName"] = _surveyDbName,
                ["LinkDbName"] = _linkDbName,
                ["SessionDbName"] = _sessionDbName,
                ["ConnectionString"] = connString
            };


            //Host + app setup
            var builder = new WebHostBuilder()
                .UseEnvironment("IntegrationTest")
                .ConfigureAppConfiguration((hostingContext, config) =>
                    config.AddInMemoryCollection(dictionary))
                .UseStartup<Startup>();


            _testServer = new TestServer(builder);
            _mongoClient = new MongoClient(connString);
            ManagerFactory = new ManagerFactory(_mongoClient, _linkDbName, _surveyDbName, _userDbName, _sessionDbName);
            HttpClient = new HttpClientWrapper(_testServer.CreateClient());
        }


        public void DropDatabases()
        {
            var databaseNames = new List<string>() { _userDbName, _surveyDbName, _linkDbName, _sessionDbName };

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

        public async Task SignInAsVolunteer(string surveyName, string linkId, string participantEmail, int participantId)
        {
            var linkModel = new SurveyLinkModel()
            {
                LinkId = linkId,
                SurveyName = surveyName,
                ParticipantId = participantId,
                ParticipantEmail = participantEmail
            };

            var manager = ManagerFactory.CreateSurveyLinkManager();
            await manager.SaveSurveyLink(linkModel);


            var verifyLinkIdModel = new VerifyLinkIdModel() { LinkId = linkModel.LinkId };
            var content = JsonConvert.SerializeObject(verifyLinkIdModel);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("Cookie/VerifyLinkId", UriKind.Relative),
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };


            var response = await HttpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
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

        public UserModel GetResearcherUserModel()
        {
            return new UserModel()
            {
                Email = "researcher@test.test",
                FirstName = "researcher",
                LastName = "researcher",
                Role = Role.Researcher,
                Country = "test",
                Institution = "test"
            };
        }

        public UserModel GetAdminUserModel()
        {
            return new UserModel()
            {
                Email = "admin@test.test",
                FirstName = "admin",
                LastName = "admin",
                Role = Role.Administrator,
                Country = "test",
                Institution = "test"
            };
        }
    }
}
