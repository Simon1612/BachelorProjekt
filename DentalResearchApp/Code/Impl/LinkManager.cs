using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DentalResearchApp.Code.Impl
{
    public class LinkManager : ILinkManager
    {
        private readonly IMongoDatabase _db;

        public LinkManager(IMongoClient client, string databaseName)
        {
            _db = client.GetDatabase("LinkDb");
        }

        public async Task<SurveyLinkModel> GetSurveyLink(string linkId)
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            return await collection.AsQueryable().FirstOrDefaultAsync(x => x.LinkId == linkId);
        }

        public async Task SendSurveyLink(string surveyName, string participantId, string baseUrl)
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            var linkId = Guid.NewGuid().ToString("N");

            var link = new SurveyLinkModel()
            {
                LinkId = linkId,
                SurveyName = surveyName,
                ParticipantId = participantId
            };

            await collection.InsertOneAsync(link);


            //Send emails here?
            var actionUrl = "/surveyrunner/index?id=";
            var surveyLink = baseUrl + actionUrl + linkId;

            //TODO: Create and send email?
        }

        public async Task DeleteSurveyLink(string linkId)
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            await collection.DeleteOneAsync(x => x.LinkId == linkId);
        }


        public void SeedWithDefaultLinks()
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            var defaultLink = new SurveyLinkModel()
            {
                LinkId = Guid.Empty.ToString(),
                SurveyName = "IncomeSurvey",
                ParticipantId = Guid.NewGuid().ToString()
            };

            collection.InsertOne(defaultLink);
        }
    }
}
