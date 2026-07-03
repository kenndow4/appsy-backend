namespace appsy.src.service;

using System.Text.Json;
using appsy.src.dtos;
using appsy.src.repository;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

public class BoardService(BoardRepository boardRepository)
{
    private readonly BoardRepository _boardRepository = boardRepository;

    // Crear board nuevo

    public async Task<IEnumerable<GetAllBoardResponse>> GetAllBoards(string userId)
    {
        var boards = await _boardRepository.GetAllByUserId(userId);
        Console.WriteLine($"Boards retrieved for user {userId}: {boards.Count()}");
        Console.WriteLine($"Boards retrieved for user {userId}: boards");

        return boards.Select(board => new GetAllBoardResponse
        {
            Id = board.Id!,
            UserId = board.UserId,
            Title = board.Title,
            CreatedAt = board.CreatedAt,
            UpdatedAt = board.UpdatedAt
        });
    }

    public async Task<BoardDto> CreateBoard(
        string userId,
        string title
    )
    {
        var board = new BoardDto
        {
            UserId = userId,
            Title = title,
            Elements = []
        };

        return await _boardRepository.Create(board);
    }

    // Guardar cambios de Excalidraw
  public async Task<object?> UpdateBoard(
    string boardId,
    JsonElement elements
)
{
    var existingBoard =
        await _boardRepository.GetById(boardId);

    if (existingBoard == null)
        return null;

    var bson = BsonSerializer.Deserialize<BsonArray>(
        elements.GetRawText()
    );

    await _boardRepository.Update(
        boardId,
        bson
    );

    return new
    {
        Saved = true
    };
}

    // Obtener board
   public async Task<object?> GetBoard(
    string boardId
)
{
    var board = await _boardRepository.GetById(boardId);

    if (board == null)
        return null;

    return new
    {
        Id = board.Id,
        UserId = board.UserId,
        Title = board.Title,

        Elements = JsonSerializer.Deserialize<object>(
            board.Elements.ToJson()
        ),

        CreatedAt = board.CreatedAt,
        UpdatedAt = board.UpdatedAt
    };
}
}