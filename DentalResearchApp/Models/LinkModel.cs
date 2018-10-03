using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class LinkModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public Guid LinkId { get; set; }
        public string SurveyName { get; set; }
    }
}
