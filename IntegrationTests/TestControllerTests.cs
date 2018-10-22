using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{

    public class PingControllerTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;

        public PingControllerTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task PingControllerHappyPath()
        {
            var response = await _fixture.Client.GetAsync("test");

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(responseStrong);
            Assert.Equal("ayy", responseStrong);
        }
    }
}
