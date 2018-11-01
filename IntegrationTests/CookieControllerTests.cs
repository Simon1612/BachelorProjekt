using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests
{
    public class CookieControllerTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly TestFixture _fixture;

        public CookieControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.DropDatabases();
        }

        [Theory]
        [InlineData("admin@test.test", "admin")]
        [InlineData("researcher@test.test", "researcher")]
        public async Task CookieController_LoginWithValidCredentials_CookieIsReturned(string userName, string passWord)
        {
            await _fixture.SeedWithUsers();

            var loginModel = new LoginModel { Username = userName, Password = passWord };
            var content = JsonConvert.SerializeObject(loginModel);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("Cookie/Login", UriKind.Relative),
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            var response = await _fixture.HttpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


            response.Headers.TryGetValues("Set-Cookie", out var newCookies);
            var cookieList = newCookies.ToList();

            Assert.Single(cookieList);
            Assert.Contains("auth_cookie", cookieList.First());
        }

        [Fact]
        public async Task CookieController_LoginWithInvalidCredentials_NoCookieIsReturned()
        {
            await _fixture.SeedWithUsers();

            var loginModel = new LoginModel { Username = "InvalidUserName", Password = "InvalidPassword" };
            var content = JsonConvert.SerializeObject(loginModel);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("Cookie/Login", UriKind.Relative),
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            var response = await _fixture.HttpClient.SendAsync(request);

            response.Headers.TryGetValues("Set-Cookie", out var newCookies);

            Assert.Null(newCookies);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task CookieController_VerifyLinkIdThatExists_CookieIsReturned()
        {
            var linkId = "LinkId";
            var participantEmail = "test@test.dk";
            var id = "id";
            var surveyName = "TestSurvey";


            var linkModel = new SurveyLinkModel()
            {
                LinkId = linkId,
                SurveyName = surveyName,
                ParticipantId = id,
                ParticipantEmail = participantEmail
            };

            var linkManager = _fixture.ManagerFactory.CreateSurveyLinkManager();
            await linkManager.SaveSurveyLink(linkModel);


            var verifyLinkIdModel = new VerifyLinkIdModel(){LinkId = linkId};
            var content = JsonConvert.SerializeObject(verifyLinkIdModel);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("Cookie/VerifyLinkId", UriKind.Relative),
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            var response = await _fixture.HttpClient.SendAsync(request);


            response.Headers.TryGetValues("Set-Cookie", out var newCookies);
            var cookieList = newCookies.ToList();

            Assert.Single(cookieList);
            Assert.Contains("auth_cookie", cookieList.First());
        }

        [Fact]
        public async Task CookieController_VerifyLinkIdThatDoesNotExist_NoCookieIsReturned()
        {
            var verifyLinkIdModel = new VerifyLinkIdModel() { LinkId = "TestId" };
            var content = JsonConvert.SerializeObject(verifyLinkIdModel);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("Cookie/VerifyLinkId", UriKind.Relative),
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            var response = await _fixture.HttpClient.SendAsync(request);


            response.Headers.TryGetValues("Set-Cookie", out var newCookies);


            Assert.Null(newCookies);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
