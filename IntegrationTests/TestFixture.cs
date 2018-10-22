using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DentalResearchApp;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Headers;
using Mongo2Go;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationTests
{
    public class TestFixture : IDisposable
    {
        public readonly CookieContainer Cookies = new CookieContainer();
        public readonly TestServer _testServer;
        public HttpClient HttpClient { get; }

        public MongoClient MongoClient { get; set; }

        private MongoDbRunner _dbRunner;

        public TestFixture()
        {
            // Connect to local mongodb
            var connString = CreateConnection();

            // Create and overwrite appsettings.Json
            dynamic rootObject = new JObject();
            dynamic mongoConnection = new JObject();

            mongoConnection.ConnectionString = connString;
            rootObject.MongoConnection = mongoConnection;

            var testConfig = rootObject.ToString();
            File.WriteAllText("appsettings.IntegrationTest.json", testConfig);


            //Host + app setup
            var builder = new WebHostBuilder()
                .UseEnvironment("IntegrationTest")
                .ConfigureAppConfiguration((hostingContext, config) =>
                    config.AddJsonFile("appsettings.IntegrationTest.json", optional: false, reloadOnChange: true))
                .UseStartup<Startup>();

            _testServer = new TestServer(builder);
            MongoClient = new MongoClient(connString);


            HttpClient = _testServer.CreateClient();
   
            SeedWithUsers();
        }

        private async Task SeedWithUsers()
        {
            var userManager = new UserManager(MongoClient);

            var user = new UserModel()
            {
                Email = "admin@test.test",
                FirstName = "admin",
                LastName = "admin",
                Role = Role.Administrator,
            };

            var password = "admin";

            var salt = Salt.Create();
            var hash = Hash.Create(password, salt);

            var login = new UserCredentials() { UserName = user.Email, Hash = hash, Salt = salt };


            await userManager.CreateUser(user, login);
        }

        public async Task SignInAsAdmin()
        {
            var login = new LoginModel() {Username = "admin@test.test",Password = "admin"};

            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            //"OLD"
            var response = await HttpClient.PostAsync("cookie/login", content);
            var message = await response.Content.ReadAsStringAsync();

            if (!message.Contains("Success"))
                throw new UnauthorizedAccessException("Unable to login with admin user");


            if (response.Headers.TryGetValues("Set-Cookie", out var newCookies))
            {
                foreach (var item in SetCookieHeaderValue.ParseList(newCookies.ToList()))
                {
                    Cookies.Add(response.RequestMessage.RequestUri, new Cookie(item.Name.Value, item.Value.Value, item.Path.Value));
                }
            }

        }

        public string CreateConnection()
        {
            _dbRunner = MongoDbRunner.Start(singleNodeReplSet: false); // true??

            return _dbRunner.ConnectionString;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
            _testServer.Dispose();
            _dbRunner.Dispose();
        }
    }
}
