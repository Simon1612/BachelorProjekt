using System;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DentalResearchApp.Code.Impl
{
    public class SurveyLinkManager : ISurveyLinkManager
    {
        private readonly IMongoDatabase _db;

        public SurveyLinkManager(IMongoClient client, string databaseName)
        {
            _db = client.GetDatabase(databaseName);
        }

        public async Task<SurveyLinkModel> GetLink(string linkId)
        {
            var collection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            return await collection.AsQueryable().FirstOrDefaultAsync(x => x.LinkId == linkId);
        }

        public async Task SendLink(string surveyName, string participantEmail, string participantId, string baseUrl)
        {
            var linkId = Guid.NewGuid().ToString("N");

            var link = new SurveyLinkModel()
            {
                LinkId = linkId,
                SurveyName = surveyName,
                ParticipantEmail = participantEmail,
                ParticipantId = participantId
            };

            await SaveSurveyLink(link);


            //Send emails here?
            var actionUrl = "/surveyrunner/index?id=";
            var surveyLink = baseUrl + actionUrl + linkId;

            //TODO: Create and send email?
            var mailHelper = new MailHelper();

            var mailSubject = "Some not-so-random subject here";
            var mailBody = $"Some text and then a link: {surveyLink}";

            mailHelper.SendMail(link.ParticipantEmail, mailSubject, mailBody);
        }

        public async Task SaveSurveyLink(SurveyLinkModel surveyLink)
        {
            var surveyCollection = _db.GetCollection<SurveyLinkModel>("surveyLink_collection");

            await surveyCollection.InsertOneAsync(surveyLink);
        }

        public async Task DeleteLink(string linkId)
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
                ParticipantId = Guid.NewGuid().ToString(),
                ParticipantEmail = DateTime.Now.ToString("fffffff") + "@fakemail.com"
            };

            collection.InsertOne(defaultLink);
        }
    }
}
