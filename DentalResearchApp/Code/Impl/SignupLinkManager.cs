using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
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
            throw new NotImplementedException();
        }

        public async Task<SignupLinkModel> GetLink(string linkId)
        {
            throw new NotImplementedException();
        }

        public async Task SendLink(string recipiantEmail, string recipiantId, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }
}
