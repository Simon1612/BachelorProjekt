using System;
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
        public int ParticipantId { get; set; }
        public DateTime TimeStamp { get; set; }
        public ObjectId UserSessionId { get; set; }

        public Survey Survey
        {
            get => default(Survey);
            set
            {
            }
        }

        public ResultLink ResultLink
        {
            get => default(ResultLink);
            set
            {
            }
        }
    }
}
