using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DentalResearchApp.Code.Impl
{
    public class SurveyManager : ISurveyManager
    {
        private readonly IMongoDatabase _db;

        public SurveyManager()
        {
            var client = new MongoClient("mongodb+srv://test:test@2018e21-surveydb-wtdmw.mongodb.net/test?retryWrites=true");
            _db = client.GetDatabase("SurveyDb");

            if (!_db.ListCollectionNames().Any())
                SeedWithDefaultSurveys();
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

            return new Dictionary<string, string> { { survey?.SurveyName, survey?.Json } };
        }


        public async Task<Dictionary<string, string>> GetAllSurveys()
        {
            var coll = _db.GetCollection<Survey>("survey_collection");
            var surveys = await coll.AsQueryable().ToListAsync();

            Dictionary<string, string> surveysdDictionary = new Dictionary<string, string>();

            foreach (var survey in surveys)
            {
                surveysdDictionary[survey.SurveyName] = survey.Json;
            }

            return surveysdDictionary;
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
