using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Xunit;

namespace IntegrationTests
{

    public class PingControllerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public PingControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task PingControllerHappyPath()
        {
            var response = await _fixture.HttpClient.GetAsync("test");

            response.EnsureSuccessStatusCode();

            var responseStrong = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(responseStrong);
            Assert.Equal("ayy", responseStrong);
        }

        [Fact]
        public async Task TestTestTest()
        {
            var db = _fixture.MongoClient.GetDatabase("Test");
            var collection = db.GetCollection<TestModel>("TestCollection");
            await collection.InsertOneAsync(new TestModel { Test = "Test" });

            var data = await collection.AsQueryable().FirstOrDefaultAsync(x => x.Test == "Test");


            Assert.NotNull(data);
            Assert.Equal("Test", data.Test);
        }
    }
}
