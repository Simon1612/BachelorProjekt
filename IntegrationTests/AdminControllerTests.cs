using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using IntegrationTests.Helpers;
using Xunit;

namespace IntegrationTests
{
    [Collection("Sequential")]
    public class AdminControllerTests : IClassFixture<TestFixture>, IDisposable
    {
        private readonly ITestFixture _fixture;

        public AdminControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        public void Dispose()
        {
            _fixture.DropDatabases();
        }

        [Fact]
        public async Task AdminController_ChangeUserRole_ResearcherIsNowAdmin()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsAdmin();

            var testUser = _fixture.GetResearcherUserModel();
            testUser.Role = Role.Administrator;

            var userEnum = new List<KeyValuePair<string, string>>()
            {
                KeyValuePair.Create("FirstName", testUser.FirstName),
                KeyValuePair.Create("LastName", testUser.LastName),
                KeyValuePair.Create("Email", testUser.Email),
                KeyValuePair.Create("Role", testUser.Role.ToString()),
                KeyValuePair.Create("Institution", testUser.Institution),
                KeyValuePair.Create("Country", testUser.Country),
            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"Admin/ChangeUserRole", UriKind.Relative),
                Content = new FormUrlEncodedContent(userEnum)
            };

            await _fixture.HttpClient.SendAsyncWithCookie(request, "login");

            var userManager = _fixture.ManagerFactory.CreateUserManager();
            var updatedUser = await userManager.GetUserModel(testUser.Email);

            Assert.NotNull(updatedUser);
            Assert.True(updatedUser.Email == testUser.Email);
            Assert.True(updatedUser.Role == Role.Administrator);
        }

        [Fact]
        public async Task AdminController_DeleteUser_UserIsDeleted()
        {
            await _fixture.SeedWithUsers();
            await _fixture.SignInAsAdmin();

            var userManager = _fixture.ManagerFactory.CreateUserManager();
            var user = await userManager.GetUserModel("researcher@test.test");

            Assert.NotNull(user);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"Admin/DeleteUser?email={user.Email}", UriKind.Relative),
            };

            await _fixture.HttpClient.SendAsyncWithCookie(request, "login");

            var updatedUser = await userManager.GetUserModel(user.Email);

            Assert.Null(updatedUser);
        }
    }
}
