using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.Models
{
    public class Author
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public Author()
        {
            // Generate a new ObjectId
            Id = ObjectId.GenerateNewId();
        }
    }
}
