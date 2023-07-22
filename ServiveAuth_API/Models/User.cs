using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServiceAuth_API.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Status { get; set; }
        public List<string>? Roles { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set;}
        public DateTime? LastModifiedBy { get; set;}
    }
}