using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalResearchApp.Code.Interfaces;
using DentalResearchApp.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

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
            var participantSessionCollection = _db.GetCollection<ParticipantSessionModel>("participant_session_collection");

            var studySession = new StudySessionModel
            {
                SessionName = studySessionModel.SessionName,
                StudyId = studySessionModel.StudyId,
                Surveys = studySessionModel.Surveys,
                Participants = studySessionModel.Participants
            };

            var participantList = new List<ParticipantSessionModel>();
            foreach (var participant in studySessionModel.Participants)
            {
                participantList.Add(new ParticipantSessionModel
                {
                    StudyId = studySessionModel.StudyId,
                    ParticipantId = participant
                });
            }

            await participantSessionCollection.InsertManyAsync(participantList);
            await studySessionCollection.InsertOneAsync(studySession);
        }

        public async Task<StudySessionModel> GetStudySession(int studyId, string sessionName)
        {
            var coll = _db.GetCollection<StudySessionModel>("study_session_collection");

            var session = await coll.AsQueryable().FirstOrDefaultAsync(x => x.StudyId.Equals(studyId) && x.SessionName.Equals(sessionName));

            return session;
        }

        public List<string> GetAllSessionsForStudy(int studyId)
        {
            var coll = _db.GetCollection<StudySessionModel>("study_session_collection");
            var sessions = coll.AsQueryable().Where(x => x.StudyId.Equals(studyId)).Select(y => y.SessionName).ToList();

            return sessions;
        }

    }
}
