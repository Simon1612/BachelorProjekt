using System;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Bson;
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

        public async Task SendLink(SurveyLinkModel link, string baseUrl)
        {



            await SaveSurveyLink(link);


            //Send emails here?
            var actionUrl = "/surveyrunner/index?id=";
            var surveyLink = baseUrl + actionUrl + link.LinkId;

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

            var list = await collection.AsQueryable().ToListAsync();

            await collection.DeleteOneAsync(x => x.LinkId == linkId);
        }
    }
}
