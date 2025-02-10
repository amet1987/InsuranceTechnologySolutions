using MongoDB.Bson.Serialization.Attributes;
using Shared.Classes;

namespace Persistence.Entities;

public class Claim : Entity
{
    [BsonElement("coverId")]
    public string CoverId { get; set; }

    [BsonElement("created")]
    [BsonDateTimeOptions(DateOnly = true)]
    public DateTime Created { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("claimType")]
    public ClaimType Type { get; set; }

    [BsonElement("damageCost")]
    public decimal DamageCost { get; set; }
}
