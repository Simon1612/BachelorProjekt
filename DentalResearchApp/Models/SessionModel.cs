using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class SessionModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string StudyId { get; set; }
        public string PatientId { get; set; }
    }
}
