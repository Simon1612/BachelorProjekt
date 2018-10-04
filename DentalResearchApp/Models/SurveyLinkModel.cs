using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class SurveyLinkModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string VolunteerId { get; set; }
        public string LinkId { get; set; }
        public string SurveyName { get; set; }
    }
}
