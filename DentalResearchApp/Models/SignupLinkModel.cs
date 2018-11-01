using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class SignupLinkModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string RecipiantEmail { get; set; }
        public string LinkId { get; set; }
    }
}
