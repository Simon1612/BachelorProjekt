using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DentalResearchApp.Code.Impl
{
    public class LinkManager
    {
        private readonly IMongoDatabase _db;

        public LinkManager()
        {
            var client = new MongoClient("mongodb+srv://test:test@2018e21-surveydb-wtdmw.mongodb.net/test?retryWrites=true");
            _db = client.GetDatabase("LinkDb");

            //if (!_db.ListCollectionNames().Any())
            //    SeedWithDefaultLinks();
        }

        public async Task<SurveyLinkModel> GetSurveyLink(string linkId)
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            return await collection.AsQueryable().FirstOrDefaultAsync(x => x.LinkId == linkId);
        }

        public async Task GenerateSurveyLink(string surveyName, string volunteerId)
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            var link = new SurveyLinkModel()
            {
                LinkId = Guid.NewGuid().ToString("N"),
                SurveyName = surveyName,
                VolunteerId = volunteerId
            };

            await collection.InsertOneAsync(link);
        }

        public async Task DeleteSurveyLink(string linkId)
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            await collection.DeleteOneAsync(x => x.LinkId == linkId);
        }


        public void SeedWithDefaultLinks()
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            var defaultLink = new SurveyLinkModel() { LinkId = Guid.Empty.ToString(), SurveyName = "IncomeSurvey", VolunteerId = "asdf" };

            collection.InsertOne(defaultLink);
        }
    }
}
