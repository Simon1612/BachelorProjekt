using System.Collections.Generic;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;
using MongoDB.Bson;

namespace DentalResearchApp.Code.Impl
{
    public class SessionManager : ISessionManager
    {
        private readonly IMongoDatabase _db;

        public SessionManager(IMongoClient client, string databaseName)
        {
            _db = client.GetDatabase(databaseName);
        }


        public async Task<List<UserSession>> GetAllUserSessionsForStudySession(ObjectId studySessionId)
        {
            var userSessionCollection = _db.GetCollection<UserSession>("user_session_collection");

            return await userSessionCollection.AsQueryable().Where(x => x.StudySessionId == studySessionId).ToListAsync();
        }


        public async Task<List<UserSession>> GetAllUserSessionsForStudySession(ObjectId studySessionId, int participantId)
        {
            var userSessionCollection = _db.GetCollection<UserSession>("user_session_collection");

            return await userSessionCollection.AsQueryable().Where(x => x.StudySessionId == studySessionId && x.ParticipantId == participantId).ToListAsync();
        }

        public async Task CreateStudySession(StudySessionModel studySessionModel)
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

        public async Task CreateUserSession(UserSession userSessionModel)
        {
            var userSessionCollection = _db.GetCollection<UserSession>("user_session_collection");

            await userSessionCollection.InsertOneAsync(userSessionModel);
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



        public async Task SetStudySessionStarted(int studyId, string sessionName)
        {
            var coll = _db.GetCollection<StudySessionModel>("study_session_collection");

            var update = Builders<StudySessionModel>.Update.Set(x => x.IsStarted, true);

            await coll.UpdateOneAsync(x => x.StudyId == studyId && x.SessionName == sessionName, update);
        }


        public List<string> GetAllStudySessionsNamesForStudy(int studyId)
        {
            var coll = _db.GetCollection<StudySessionModel>("study_session_collection");
            var sessions = coll.AsQueryable().Where(x => x.StudyId.Equals(studyId));
            return new List<string>();
        }

        public List<StudySessionModel> GetAllSessionModelsForStudy(int studyId)
        {
            var coll = _db.GetCollection<StudySessionModel>("study_session_collection");
            var sessions = coll.AsQueryable().Where(x => x.StudyId.Equals(studyId)).ToList();

            return sessions;
        }
    }
}
