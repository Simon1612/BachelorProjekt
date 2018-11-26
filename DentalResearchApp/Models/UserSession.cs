using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class UserSession
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int ParticipantId { get; set; }
        public ObjectId StudySessionId { get; set; }
    }
}
