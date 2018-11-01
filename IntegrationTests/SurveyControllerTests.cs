using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using IntegrationTests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests
{
    public class SurveyControllerTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly ITestFixture _fixture;

        public SurveyControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.DropDatabases();
        }

        [Fact]
        public async Task SurveyController_CreateSurvey_EmptySurveyCreated()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsResearcher();

            var surveyName = "testSurvey";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"Survey/Create?name={surveyName}", UriKind.Relative)
            };


            await _fixture.HttpClient.SendAsyncWithCookie(request, "login");


            var surveyManager = _fixture.ManagerFactory.CreateSurveyManager();
            var createdSurvey = await surveyManager.GetSurvey("testSurvey");


            Assert.NotNull(createdSurvey);
            Assert.Single(createdSurvey);
            Assert.True(createdSurvey.ContainsKey(surveyName));
            Assert.True(createdSurvey.ContainsValue("{}"));
        }


        [Fact]
        public async Task SurveyController_GetActiveAsync_AllSurveysReturned()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsResearcher();

            var surveyName1 = "testSurvey1";
            var surveyName2 = "testSurvey2";

            var manager = _fixture.ManagerFactory.CreateSurveyManager();
            await manager.CreateSurvey(surveyName1);
            await manager.CreateSurvey(surveyName2);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("Survey/getActive", UriKind.Relative)
            };


            var response = await _fixture.HttpClient.SendAsyncWithCookie(request, "login");

            var content = await response.Content.ReadAsStringAsync();
            var surveys = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);


            Assert.NotNull(surveys);
            Assert.Equal(2, surveys.Count);
            Assert.Contains("{}", surveys[surveyName1]);
            Assert.Contains("{}", surveys[surveyName2]);
        }

        [Fact]
        public async Task SurveyController_GetSurvey_SurveyReturned()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsResearcher();

            var surveyName = "testSurvey";

            var manager = _fixture.ManagerFactory.CreateSurveyManager();

            await manager.CreateSurvey(surveyName);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"Survey/GetSurvey?surveyId={surveyName}", UriKind.Relative)
            };

            var response = await _fixture.HttpClient.SendAsyncWithCookie(request, "login");
            var content = await response.Content.ReadAsStringAsync();

            Assert.NotNull(content);
            Assert.Contains("{}", content);
        }


        [Fact]
        public async Task SurveyController_DeleteSurvey_SurveyDeleted()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsResearcher();

            var manager = _fixture.ManagerFactory.CreateSurveyManager();

            var surveyName = "deleteMe";
            await manager.CreateSurvey(surveyName);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"Survey/delete?Id={surveyName}", UriKind.Relative)
            };

            var response = await _fixture.HttpClient.SendAsyncWithCookie(request, "login");

            var surveys = await manager.GetAllSurveys();

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(surveys);
            Assert.Empty(surveys);
        }


        [Fact]
        public async Task SurveyController_GetResults_AllSurveyResultsReturned()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsResearcher();

            var manager = _fixture.ManagerFactory.CreateSurveyManager();

            var surveyName = "name";
            var date = DateTime.Now;

            var result1 = new SurveyResult { JsonResult = "{}", SurveyName = surveyName, TimeStamp = date };
            var result2 = new SurveyResult { JsonResult = "{}", SurveyName = surveyName, TimeStamp = date };


            await manager.SaveSurveyResult(result1);
            await manager.SaveSurveyResult(result2);


            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"Survey/GetResults?postId={surveyName}", UriKind.Relative)
            };


            var response = await _fixture.HttpClient.SendAsyncWithCookie(request, "login");
            var content = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<List<string>>(content);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.Equal(2, results.Count);
            Assert.All(results, result => Assert.Contains("{}", result));
        }


        [Fact]
        public async Task SurveyController_RequestWithNoCookie_UserIsRedirected()
        {
            var surveyName = "testSurvey";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"Survey/Create?name={surveyName}", UriKind.Relative)
            };

            var response = await _fixture.HttpClient.SendAsync(request);
            
            
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }
    }
}
