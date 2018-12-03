using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DentalResearchApp.Models
{
    public class UserModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Institution { get; set; }
        public string Country { get; set; }

        public UserCredentials UserCredentials
        {
            get => default(UserCredentials);
            set
            {
            }
        }
    }
}
