using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace SafeCampus.Models;

public class Track
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { set; get; }

    [BsonElement("laptop_id")]
    public string? LaptopId { set; get; }

    [BsonElement("guard_id")]
    public string? GuardId { set; get; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}