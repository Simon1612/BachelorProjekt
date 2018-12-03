using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class ParticipantSessionModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string ParticipantId { get; set; }
        public int StudyId { get; set; }
    }
}
