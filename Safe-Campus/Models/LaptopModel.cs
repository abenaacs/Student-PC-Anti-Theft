using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SafeCampus.Models;



public class Laptop
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("ownerId")]
    public string? OwnerId { get; set; }

    [BsonElement("brand")]
    public string? Brand { get; set; }

    [BsonElement("model")]
    public string? Model { get; set; }

    [BsonElement("serialNumber")]
    public string? SerialNumber { get; set; }


}