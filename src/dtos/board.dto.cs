namespace appsy.src.dtos;

using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class BoardDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Title { get; set; } = "Untitled";

    // Aquí guardas todo el JSON de Excalidraw
    public BsonArray Elements { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}






public class UpdateBoardDto
{
    public JsonElement Elements { get; set; }
}


public class CreateBoardRequest
{
    public string Title { get; set; } = null!;
}

public class GetAllBoardResponse
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}