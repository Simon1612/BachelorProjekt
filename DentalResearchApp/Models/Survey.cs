using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class Survey
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Json { get; set; }

        public string SurveyName { get; set; }
    }
}
