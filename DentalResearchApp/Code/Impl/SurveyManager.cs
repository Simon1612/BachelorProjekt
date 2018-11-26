using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DentalResearchApp.Code.Impl
{
    public class SurveyManager : ISurveyManager
    {
        private readonly IMongoDatabase _db;

        public SurveyManager(IMongoClient client, string databaseName)
        {
            _db = client.GetDatabase(databaseName);
        }

        public async Task CreateSurvey(string surveyName)
        {
            var survey = new Survey { Json = "{}", SurveyName = surveyName };

            var collection = _db.GetCollection<Survey>("survey_collection");

            await collection.InsertOneAsync(survey);
        }

        public async Task DeleteSurvey(string surveyName)
        {
            var coll = _db.GetCollection<Survey>("survey_collection");

            await coll.DeleteOneAsync(x => x.SurveyName == surveyName);
        }

        public async Task<Dictionary<string, string>> GetSurvey(string surveyName)
        {
            var coll = _db.GetCollection<Survey>("survey_collection");

            var survey = await coll.AsQueryable().FirstOrDefaultAsync(s => s.SurveyName == surveyName);

            var dictionary = new Dictionary<string, string> {[surveyName] = survey?.Json ?? ""};

            return dictionary;
        }


        public async Task<Dictionary<string, string>> GetAllSurveys()
        {
            var coll = _db.GetCollection<Survey>("survey_collection");

            var surveys = await coll.AsQueryable().ToListAsync();

            Dictionary<string, string> surveysDictionary = new Dictionary<string, string>();

            foreach (var survey in surveys)
            {
                surveysDictionary[survey.SurveyName] = survey.Json;
            }

            return surveysDictionary;
        }

        public async Task ChangeSurvey(ChangeSurveyModel model)
        {
            var coll = _db.GetCollection<Survey>("survey_collection");

            var update = Builders<Survey>.Update.Set(s => s.Json, model.Json);

            await coll.UpdateOneAsync(s => s.SurveyName == model.Id, update);
        }

        public async Task ChangeSurveyName(string id, string name)
        {
            var coll = _db.GetCollection<Survey>("survey_collection");

            var update = Builders<Survey>.Update.Set(s => s.SurveyName, name);

            await coll.UpdateOneAsync(s => s.SurveyName == id, update);
        }

        public async Task SaveSurveyResult(SurveyResult result)
        {
            var coll = _db.GetCollection<SurveyResult>("surveyResult_collection");

            await coll.InsertOneAsync(result);
        }


        public async Task<List<string>> GetResults(string postId)
        {
            var coll = _db.GetCollection<SurveyResult>("surveyResult_collection");
            var results = await coll.FindAsync(x => x.SurveyName == postId);

            var resultList = new List<string>();

            foreach (var result in results.ToList())
            {
                resultList.Add(result.JsonResult);
            }

            return resultList;
        }

        public async Task<List<string>> GetResultsForUserSessionSurvey(int participantId, ObjectId userSessionId, string surveyName)
        {
            var coll = _db.GetCollection<SurveyResult>("surveyResult_collection");
            var results = await coll.FindAsync(x => x.SurveyName == surveyName && x.UserSessionId == userSessionId && x.ParticipantId == participantId);

            var resultList = new List<string>();

            foreach (var result in results.ToList())
            {
                resultList.Add(result.JsonResult);
            }

            return resultList;
        }


        public async Task<List<string>> GetAllNames()
        {
            var coll = _db.GetCollection<Survey>("survey_collection");
            var surveys = await coll.AsQueryable().ToListAsync();
            var surveyNamesList = new List<string>();

            foreach (var survey in surveys)
            {
                surveyNamesList.Add(survey.SurveyName);
            }

            return surveyNamesList;
        }


        private async void SeedWithDefaultSurveys()
        {
            //string json1 = System.IO.File.ReadAllText(@"Views/Survey/Json/IncomeSurvey.json");
            string json2 = System.IO.File.ReadAllText(@"Views/Survey/Json/ProductFeedbackSurvey.json");

            //var survey1 = new Survey() {Json = json1, SurveyName = "IncomeSurvey" };
            var survey2 = new Survey() { Json = json2, SurveyName = "ProductFeedbackSurvey" };

            var collection = _db.GetCollection<Survey>("survey_collection");

            await collection.InsertManyAsync(new[] { survey2 });
        }
    }
}
