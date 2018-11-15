using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DentalResearchApp.Code.Impl
{
    public class SessionManager : ISessionManager
    {
        private readonly IMongoDatabase _db;

        public SessionManager(IMongoClient client, string databaseName)
        {
            _db = client.GetDatabase(databaseName);
        }

        public async Task CreateSession(StudySessionModel studySessionModel)
        {
            var studySessionCollection = _db.GetCollection<StudySessionModel>("study_session_collection");

            var studySession = new StudySessionModel
            {
                SessionName = studySessionModel.SessionName,
                StudyId = studySessionModel.StudyId,
                Surveys = studySessionModel.Surveys,
            };

            await studySessionCollection.InsertOneAsync(studySession);
        }

        public async Task<StudySessionModel> GetStudySession(int studyId, string sessionName)
        {
            var coll = _db.GetCollection<StudySessionModel>("study_session_collection");

            var session = await coll.AsQueryable().FirstOrDefaultAsync(x => x.StudyId.Equals(studyId) && x.SessionName.Equals(sessionName));

            return session;
        }

        public async Task DeleteSession(StudySessionModel studySessionModel)
        {
            var studySessionCollection = _db.GetCollection<StudySessionModel>("study_session_collection");

            await studySessionCollection.DeleteOneAsync(x => x.Id == studySessionModel.Id);
        }

        public List<string> GetAllSessionsForStudy(int studyId)
        {
            var coll = _db.GetCollection<StudySessionModel>("study_session_collection");
            var sessions = coll.AsQueryable().Where(x => x.StudyId.Equals(studyId));
            if (!sessions.Equals(null))
            {
               var sessionsList =  sessions.Select(y => y.SessionName).ToList();
                return sessionsList;
            }
            return new List<string>();
        }

    }
}
