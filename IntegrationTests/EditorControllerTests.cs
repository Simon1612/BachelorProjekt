using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using IntegrationTests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests
{
    [Collection("Sequential")]
    public class EditorControllerTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly ITestFixture _fixture;

        public EditorControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.DropDatabases();
        }

        [Fact]
        public async Task EditorController_ChangeJson_SurveyCorrectlyChanged()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsResearcher();

            var surveyName1 = "testSurvey1";

            var manager = _fixture.ManagerFactory.CreateSurveyManager();
            await manager.CreateSurvey(surveyName1);

            var newJson = "{'test': 'test'}";
            var resultModel = new ChangeSurveyModel { Json =  newJson, Id = surveyName1 };
            var jsonContent = JsonConvert.SerializeObject(resultModel);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("Editor/changeJson", UriKind.Relative),
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            var response = await _fixture.HttpClient.SendAsyncWithCookie(request, "login");
            var content = await response.Content.ReadAsStringAsync();

            var survey = await manager.GetSurvey(surveyName1);
            var surveyString = JsonConvert.SerializeObject(survey);

            Assert.NotNull(survey);
            Assert.Equal("", content);
            Assert.Contains(newJson, surveyString);
        }

        [Fact]
        public async Task EditorController_ChangeName_SurveyCorrectlyChanged()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsResearcher();

            var surveyName1 = "testSurvey1";
            var newSurveyName = "newSurveyName";
            var testJson = "{'test': 'test'}";

            var manager = _fixture.ManagerFactory.CreateSurveyManager();
            await manager.CreateSurvey(surveyName1);
            await manager.ChangeSurvey(new ChangeSurveyModel
            {
                Id = surveyName1,
                Json = testJson
            });

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"Editor/changeName?id={surveyName1}&name={newSurveyName}", UriKind.Relative),
            };

            await _fixture.HttpClient.SendAsyncWithCookie(request, "login");

            var nullSurvey = await manager.GetSurvey(surveyName1);

            Assert.False(nullSurvey.ContainsValue(testJson));

            var survey = await manager.GetSurvey(newSurveyName);
            var surveyString = JsonConvert.SerializeObject(survey);

            Assert.NotNull(survey);
            Assert.Contains(newSurveyName, surveyString);
        }
    }
}
