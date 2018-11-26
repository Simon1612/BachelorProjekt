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

            var mailSubject = "Signup Invitation - Department of Dentistry and Oral Health";
            var mailBody = $"You have been invited to sign up for Department of Dentistry and Oral Health's survey tool.\n" +
                $"Follow this link to sign up: {surveyLink}\n\n" +
                $"Best regards Department of Dentistry and Oral Health,\n Vennelyst Blvd. 9,\n 8000 Aarhus C";

            mailHelper.SendMail(link.RecipiantEmail, mailSubject, mailBody);
        }
    }
}
