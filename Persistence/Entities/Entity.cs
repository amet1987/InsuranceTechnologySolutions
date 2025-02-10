using MongoDB.Bson.Serialization.Attributes;

namespace Persistence.Entities;

public class Entity
{
    [BsonId]
    public string Id { get; set; }
}
