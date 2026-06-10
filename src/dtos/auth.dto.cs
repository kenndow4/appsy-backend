using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace appsy.src.dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(3)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(5)]
        public string Password { get; set; } = string.Empty;

        // [Required]
        // [MinLength(5)]

        // public string ConfirmPassword { get; set; } = string.Empty;
    }
}
