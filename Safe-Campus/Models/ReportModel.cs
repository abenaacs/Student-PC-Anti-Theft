using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeCampus.Models;
public class Report
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { set; get; }

    [BsonElement("Owner_id")]
    public string? OwnerId { set; get; }

    [BsonElement("issue")]
    public string? Issue { set; get; }

    [BsonElement("information")]
    public string? Information { set; get; }

    [BsonElement("status")]
    public string Status { set; get; } = "Ongoing";

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}