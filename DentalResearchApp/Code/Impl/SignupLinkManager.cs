using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace DentalResearchApp.Code.Impl
{
    public class SignupLinkManager : ISignupLinkManager
    {
        private readonly IMongoDatabase _db;

        public SignupLinkManager(IMongoClient client, string databaseName)
        {
            _db = client.GetDatabase(databaseName);
        }

        public async Task DeleteLink(string linkId)
        {
            var collection = _db.GetCollection<SignupLinkModel>("signupLink_collection");

            await collection.DeleteOneAsync(x => x.LinkId == linkId);
        }

        public async Task<SignupLinkModel> GetLink(string linkId)
        {
            var collection = _db.GetCollection<SignupLinkModel>("signupLink_collection");

            return await collection.AsQueryable().FirstOrDefaultAsync(x => x.LinkId == linkId);
        }

        public async Task SendLink(string recipiantEmail, string baseUrl)
        {
            var surveyCollection = _db.GetCollection<SignupLinkModel>("signupLink_collection");

            var linkId = Guid.NewGuid().ToString("N");

            var link = new SignupLinkModel()
            {
                RecipiantEmail = recipiantEmail,
                LinkId = linkId
            };

            await surveyCollection.InsertOneAsync(link);

            //Send emails here?
            var actionUrl = "/login/signup?id=";
            var surveyLink = baseUrl + actionUrl + linkId;

            //TODO: Create and send email?
            var mailHelper = new MailHelper();

            var mailSubject = "Some not-so-random subject here";
            var mailBody = $"Some text and then a link: {surveyLink}";

            mailHelper.SendMail(link.RecipiantEmail, mailSubject, mailBody);
        }
    }
}
