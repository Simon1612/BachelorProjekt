using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using MongoDB.Driver;
using Xunit;

namespace IntegrationTests
{
    public class SurveyControllerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public SurveyControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public async Task SurveyController_CreateSurvey_EmptySurveyCreated()
        {
            await _fixture.SignInAsAdmin();

           
            var cookies = _fixture.Cookies.GetCookies(new Uri("http://localhost/cookie/login"));
            var authCookie = cookies["auth_cookie"];
            

            var surveyName = "testSurvey";

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri($"Survey/Create?name={surveyName}", UriKind.Relative);
            request.Headers.Add("Cookie", new CookieHeaderValue(authCookie.Name, authCookie.Value).ToString());


            var response = await _fixture.HttpClient.SendAsync(request);


            var surveyManager = new SurveyManager(_fixture.MongoClient);
            var createdSurvey = await surveyManager.GetSurvey("testSurvey");

            Assert.NotNull(createdSurvey);
            Assert.Single(createdSurvey);
            Assert.True(createdSurvey.ContainsKey(surveyName));
            Assert.True(createdSurvey.ContainsValue("{}"));
        }

    }
}
