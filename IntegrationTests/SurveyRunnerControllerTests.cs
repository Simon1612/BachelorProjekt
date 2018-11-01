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
    public class SurveyRunnerControllerTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly ITestFixture _fixture;
        public SurveyRunnerControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.DropDatabases();
        }

        [Fact]
        public async Task SurveyRunnerController_GetSurveyWithNoCookie_ReturnsRedirect()
        {
            var surveyId = "TestSurvey";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"SurveyRunner/GetSurvey?surveyId={surveyId}", UriKind.Relative),
            };

            var response = await _fixture.HttpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        }

        [Fact]
        public async Task SurveyRunnerController_GetSurveyWithCookie_ReturnsSurvey()
        {
            var surveyName = "TestSurvey";
            await _fixture.SignInAsVolunteer(surveyName, "asdf", "asdf", "asdf");

            var surveyManager = _fixture.ManagerFactory.CreateSurveyManager();
            await surveyManager.CreateSurvey(surveyName);


            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"SurveyRunner/GetSurvey?surveyId={surveyName}", UriKind.Relative),
            };

            var response = await _fixture.HttpClient.SendAsyncWithCookie(request, "VerifyLinkId");
            var content = await response.Content.ReadAsStringAsync();


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("{}", content);
        }


        [Fact]
        public async Task SurveyRunnerController_GetSurveyWithCookieButWrongSurveyClaim_ReturnsBadRequest()
        {
            var surveyName = "TestSurvey";
            var claimedSurveyName = "I am special";

            await _fixture.SignInAsVolunteer(claimedSurveyName, "asdf", "asdf", "asdf");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"SurveyRunner/GetSurvey?surveyId={surveyName}", UriKind.Relative),
            };

            var response = await _fixture.HttpClient.SendAsyncWithCookie(request, "VerifyLinkId");
        

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task SurveyRunnerController_PostResults_ResultsAreSavedSurveyLinkIsDeleted()
        {
            var linkId = "LinkId";
            var participantEmail = "test@test.dk";
            var participantId = "id";
            var surveyName = "TestSurvey";

            var result = "{}";
            var resultModel = new PostSurveyResultModel {PostId = surveyName, SurveyResult = result};
            var jsonContent = JsonConvert.SerializeObject(resultModel);

            await _fixture.SignInAsVolunteer(surveyName, linkId, participantEmail, participantId);


            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("SurveyRunner/Post", UriKind.Relative),
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };

            var response = await _fixture.HttpClient.SendAsyncWithCookie(request, "VerifyLinkId");

            var surveyLink = await _fixture.ManagerFactory.CreateSurveyLinkManager().GetLink(linkId);
            var surveyResult = await _fixture.ManagerFactory.CreateSurveyManager().GetResults(surveyName);


            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Null(surveyLink);
            Assert.NotNull(surveyResult);
            Assert.Single(surveyResult);
            Assert.Contains(result, surveyResult);
        }

    }
}
