using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class StudySessionModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string SessionName { get; set; }
        public string StudyId { get; set; }
        public List<string> Participants { get; set; }
        public List<string> Surveys { get; set; }
    }
}
