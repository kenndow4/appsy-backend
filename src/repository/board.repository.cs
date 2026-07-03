namespace appsy.src.repository;

using appsy.src.dtos;
using MongoDB.Bson;
using MongoDB.Driver;

public class BoardRepository
{
    private readonly IMongoCollection<BoardDto> _boards;

    public BoardRepository(IMongoDatabase database)
    {
        _boards = database.GetCollection<BoardDto>("boards");
    }

    // Obtener todos los boards de un usuario
    public async Task<IEnumerable<BoardDto>> GetAllByUserId(string userId)
    {
        return await _boards
            .Find(x => x.UserId == userId)
            .ToListAsync();
    }
    public async Task<BoardDto> Create(BoardDto dto)
    {
        await _boards.InsertOneAsync(dto);
        return dto;
    }

    // Buscar board por id
    public async Task<BoardDto?> GetById(string id)
    {
        return await _boards
            .Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    // Actualizar canvas
 public async Task Update(
    string id,
    BsonArray elements
)
{
    var update = Builders<BoardDto>.Update
        .Set(x => x.Elements, elements)
        .Set(x => x.UpdatedAt, DateTime.UtcNow);

    await _boards.UpdateOneAsync(
        x => x.Id == id,
        update
    );
}
}