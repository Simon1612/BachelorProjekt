using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DentalResearchApp.Code.Impl;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NSubstitute;
using Xunit;

namespace UnitTests
{
    public class SurveyManagerTests
    {
        private readonly IMongoClient _mockClient;
        private readonly IMongoDatabase _mockDb;
        private readonly IMongoCollection<Survey> _mockSurveyCollection;
        private readonly IMongoCollection<SurveyResult> _mockSurveyResultCollection;
        private readonly IMongoQueryable<Survey> _mockQueryable;

        public SurveyManagerTests()
        {
            _mockClient = Substitute.For<IMongoClient>();
            _mockDb = Substitute.For<IMongoDatabase>();
            _mockQueryable = Substitute.For<IMongoQueryable<Survey>>();
            _mockSurveyCollection = Substitute.For<IMongoCollection<Survey>>();
            _mockSurveyResultCollection = Substitute.For<IMongoCollection<SurveyResult>>();
        }

        [Fact]
        public async Task SurveyManager_CreateSurvey_InsertOneCalled()
        {
            _mockClient.GetDatabase(Arg.Any<string>()).Returns(_mockDb);
            _mockDb.GetCollection<Survey>(Arg.Any<string>()).Returns(_mockSurveyCollection);

            var manager = new SurveyManager(_mockClient, "test");


            var surveyName = "testname";
            await manager.CreateSurvey(surveyName);

            await _mockSurveyCollection.Received(1).InsertOneAsync(Arg.Is<Survey>(s => s.SurveyName == surveyName));
        }


        [Fact]
        public async Task SurveyManager_DeleteSurvey_DeleteOneCalled()
        {
            _mockClient.GetDatabase(Arg.Any<string>()).Returns(_mockDb);
            _mockDb.GetCollection<Survey>(Arg.Any<string>()).Returns(_mockSurveyCollection);

            var manager = new SurveyManager(_mockClient, "test");

            var surveyName = "testname";

            await manager.DeleteSurvey(surveyName);

            await _mockSurveyCollection.Received(1).DeleteOneAsync(Arg.Any<ExpressionFilterDefinition<Survey>>());
        }


        [Fact]
        public async Task SurveyManager_ChangeSurvey_UpdateOneAsyncCalled()
        {
            _mockClient.GetDatabase(Arg.Any<string>()).Returns(_mockDb);
            _mockDb.GetCollection<Survey>(Arg.Any<string>()).Returns(_mockSurveyCollection);

            var manager = new SurveyManager(_mockClient, "test");

            var changeSurveyModel = new ChangeSurveyModel() { Json = "{}", Id = "test"};

            await manager.ChangeSurvey(changeSurveyModel);

            await _mockSurveyCollection.Received(1).UpdateOneAsync(Arg.Any<ExpressionFilterDefinition<Survey>>(), Arg.Any<UpdateDefinition<Survey>>());
        }

        [Fact]
        public async Task SurveyManager_ChangeSurveyName_UpdateOneAsyncCalled()
        {
            _mockClient.GetDatabase(Arg.Any<string>()).Returns(_mockDb);
            _mockDb.GetCollection<Survey>(Arg.Any<string>()).Returns(_mockSurveyCollection);

            var manager = new SurveyManager(_mockClient, "test");

            await manager.ChangeSurveyName("id", "name");

            await _mockSurveyCollection.Received(1).UpdateOneAsync(Arg.Any<ExpressionFilterDefinition<Survey>>(), Arg.Any<UpdateDefinition<Survey>>());
        }

        [Fact]
        public async Task SurveyManager_SaveSurveyResults_InsertOneAsyncCalled()
        {
            _mockClient.GetDatabase(Arg.Any<string>()).Returns(_mockDb);
            _mockDb.GetCollection<SurveyResult>(Arg.Any<string>()).Returns(_mockSurveyResultCollection);

            var manager = new SurveyManager(_mockClient, "test");

            var saveSurveyModel = new SurveyResult(){SurveyName = "TestName", JsonResult = "{}"};

            await manager.SaveSurveyResult(saveSurveyModel);

            await _mockSurveyResultCollection.Received(1).InsertOneAsync(saveSurveyModel);
        }





        //TODO: Fix?
        //[Fact]
        //public async Task SurveyManager_GetSurvey_SurveyReturned()
        //{
        //    var surveyName = "testname";
        //    var testSurvey = new Survey() { SurveyName = surveyName, Json = "{}" };

        //    var surveyList = new List<Survey> { testSurvey };


        //    var cursorMock = Substitute.For<IAsyncCursor<Survey>>();
        //    cursorMock.ToList().Returns(surveyList);

        //    _mockCollection.FindAsync(x => x.SurveyName == surveyName).Returns(cursorMock);

        //    _mockDb.GetCollection<Survey>(Arg.Any<string>()).Returns(_mockCollection);

        //    _mockClient.GetDatabase(Arg.Any<string>()).Returns(_mockDb);




        //    var manager = new SurveyManager(_mockClient, "test");


        //    var survey = await manager.GetSurvey(surveyName);


        //    _mockCollection.Received(1).AsQueryable();
        //    Assert.Equal("{}", survey[surveyName]);
        //    Assert.Single(survey);
        //}
    }
}
