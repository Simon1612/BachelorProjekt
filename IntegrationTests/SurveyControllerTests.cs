using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests
{
    public class SurveyControllerTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly TestFixture _fixture;

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
    }
}
