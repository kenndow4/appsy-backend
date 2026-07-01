using Microsoft.AspNetCore.Mvc;
namespace appsy.src.controller;
using Microsoft.AspNetCore.Authorization;
using appsy.src.dtos;
using appsy.src.service;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{

 private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }


    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetUserById()
    {
        try
        {
             var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Usuario no identificado" });
            }
            return await _authService.GetUserById(userId);
            
        }
        catch (Exception ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto dto)
    {
        try
        {
             return await _authService.Register(dto);

        }
        catch (Exception ex)
        {
            
        return Conflict(new
        {
            message = ex.Message
        });
        }
      
    }

     [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
      
   try
   {
  var result = await _authService.Login(dto);
        return Ok(result);
   }
   catch (Exception ex )
   {
   return Conflict(new{
                message = ex.Message
            });
   };
        
    }
}