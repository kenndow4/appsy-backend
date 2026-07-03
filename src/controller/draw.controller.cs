namespace appsy.src.controllers;

using Microsoft.AspNetCore.Mvc;
using appsy.src.service;
using appsy.src.dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/boards")]
public class BoardController : ControllerBase
{
    private readonly BoardService _boardService;

    public BoardController(BoardService boardService)
    {
        _boardService = boardService;
    }

    // Crear board nuevo
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBoard(
        [FromBody] CreateBoardRequest request
    )
    {
       var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Usuario no identificado" });
            }

        var board = await _boardService.CreateBoard(
            userId,
            request.Title
        );

        return Ok(board);
    }

    [HttpGet("my-boards")]
    [Authorize]
    public async Task<IActionResult> GetAllBoards()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "Usuario no identificado" });
        }

        var boards = await _boardService.GetAllBoards(userId);
        return Ok(boards);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetBoard(string id)
    {
       
        var board = await _boardService.GetBoard(id);

        if (board == null)
        {
            return NotFound(new
            {
                message = "Board not found"
            });
        }

        return Ok(board);
    }

    // Guardar cambios del canvas
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<object?>> UpdateBoard(
        string id,
        [FromBody] UpdateBoardDto dto
    )
    {
        var updated =
            await _boardService.UpdateBoard(
                id,
                dto.Elements
            );

        if (updated == null)
        {
            return NotFound(new
            {
                message = "Board not found"
            });
        }

        return Ok(new
        {
            message = "Board updated successfully"
        });
    }
}