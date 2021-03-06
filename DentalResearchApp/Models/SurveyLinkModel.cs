﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class SurveyLinkModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string ParticipantEmail { get; set; }
        public int ParticipantId { get; set; }
        public string LinkId { get; set; } = Guid.NewGuid().ToString("N");
        public string SurveyName { get; set; }
        public ObjectId UserSessionId { get; set; }
    }
}
