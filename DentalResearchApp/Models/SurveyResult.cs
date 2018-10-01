using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class SurveyResult
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string SurveyName { get; set; }
        public string JsonResult { get; set; }
        public string ParticipantId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
