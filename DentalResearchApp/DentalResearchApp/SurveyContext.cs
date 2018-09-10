using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DentalResearchApp
{
    public class SurveyContext
    {
        private readonly IMongoDatabase _db;

        public SurveyContext()
        {
            var client = new MongoClient("mongodb+srv://test:test@2018e21-surveydb-wtdmw.mongodb.net/test?retryWrites=true");
            _db = client.GetDatabase("SurveyDb");


            


            if (!_db.ListCollectionNames().Any())
                FuckItUp();
            else
            {
                //var coll =  _db.GetCollection<Survey>("test_collection").FindAsync();
            }
        }

        public async void FuckItUp()
        {
            string survey1 = System.IO.File.ReadAllText(@"Views/Survey/Json/IncomeSurvey.json");
            string survey2 = System.IO.File.ReadAllText(@"Views/Survey/Json/ProductFeedbackSurvey.json");

            var document1 = BsonSerializer.Deserialize<BsonDocument>(survey1);
            var document2 = BsonSerializer.Deserialize<BsonDocument>(survey2);

            var collection = _db.GetCollection<BsonDocument>("test_collection");

            await collection.InsertManyAsync(new[] {document1, document2});
        }
    }
}
