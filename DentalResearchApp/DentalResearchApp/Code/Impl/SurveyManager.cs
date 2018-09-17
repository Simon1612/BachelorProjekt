using System.Collections.Generic;
using System.Linq;
using DentalResearchApp.Models;
using MongoDB.Driver;

namespace DentalResearchApp.Code.Impl
{
    public class SurveyManager
    {
        private readonly IMongoDatabase _db;

        public SurveyManager()
        {
            var client = new MongoClient("mongodb+srv://test:test@2018e21-surveydb-wtdmw.mongodb.net/test?retryWrites=true");
            _db = client.GetDatabase("SurveyDb");

            //if (!_db.ListCollectionNames().Any())
            //    SeedWithDefaultSurveys();
        }


        public Dictionary<string, string> GetSurveyByName(string surveyName)
        {
            var coll = _db.GetCollection<Survey>("survey_collection");

            var survey = coll.AsQueryable().FirstOrDefault(s => s.SurveyName == surveyName);

            return new Dictionary<string, string> {{survey?.SurveyName, survey?.Json}};
        }


        public Dictionary<string, string> GetAllSurveys()
        {
            var coll = _db.GetCollection<Survey>("survey_collection");
            var surveys = coll.AsQueryable().ToList(); 

            Dictionary<string, string> surveysdDictionary = new Dictionary<string, string>();

            foreach (var survey in surveys)
            {
                surveysdDictionary[survey.SurveyName] = survey.Json;
            }

            return surveysdDictionary;
        }

        private async void SeedWithDefaultSurveys()
        {
            string json1 = System.IO.File.ReadAllText(@"Views/Survey/Json/IncomeSurvey.json");
            string json2 = System.IO.File.ReadAllText(@"Views/Survey/Json/ProductFeedbackSurvey.json");

            var survey1 = new Survey() {Json = json1, SurveyName = "IncomeSurvey" };
            var survey2 = new Survey(){Json = json2, SurveyName = "ProductFeedbackSurvey" };

            var collection = _db.GetCollection<Survey>("survey_collection");

            await collection.InsertManyAsync(new[] {survey1, survey2});
        }
    }
}
