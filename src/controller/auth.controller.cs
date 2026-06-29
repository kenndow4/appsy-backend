using Microsoft.AspNetCore.Mvc;
namespace appsy.src.controller;
using appsy.src.dtos;
using appsy.src.service;
[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{

 private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }




   

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
              var user = await _authService.Register(dto);

        return Ok(user);
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