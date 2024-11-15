using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Automate.Interfaces;

namespace Automate.Models
{
    public class User : IUser
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Username")]
        public string? Username { get; set; }

        [BsonElement("Password")]
        public string? Password { get; set; }

        [BsonElement("IsAdmin")]
        public bool IsAdmin { get; set; }
    }
}
