using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Safe_Campus.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } 
        [BsonElement("userName")]
        public string UserName { get; set; } 
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; } 
        [BsonElement("refreshToken")]
        public string RefreshToken { get; set; } 
        [BsonElement("tokenCreated")]
        public DateTime RefreshTokenCreated { get; set; }
        [BsonElement("tokenExpired")]
        public DateTime RefreshTokenExpires { get; set; }     
        [BsonElement("role")]
        public string Role { get; set; }
        [BsonElement("firstName")]
        public string? FirstName { get; set; } 
        [BsonElement("lastName")]
        public string? LastName { get; set; } 
        [BsonElement("profilePicture")]
        public string? ProfilePicture { get; set; } 
        [BsonElement("contactNumber")]
        public string? ContactNumber { get; set; } 
        [BsonElement("sex")]
        public string? Sex { get; set; } 
        [BsonElement("admissionType")]
        public string? AdmissionType { get; set; }
        [BsonElement("studyLevel")]
        public string? StudyLevel { get; set; }
        [BsonElement("department")]
        public string? Department { get; set; } 
        [BsonElement("issuedDate")]
        public DateTime? StartDate { get; set; }
        [BsonElement("EndDate")]
        public DateTime? EndDate { get; set; }
    }




}
