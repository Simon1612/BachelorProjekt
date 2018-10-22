using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace IntegrationTests
{
    public class TestModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Test { get; set; }
    }
}
