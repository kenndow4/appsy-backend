using Microsoft.AspNetCore.Mvc;
namespace appsy.src.controller;
using appsy.src.dtos;
using appsy.src.service;
[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    [HttpPost("Login")]
    public ActionResult<LoginDto> Login([FromBody]LoginDto loginDto)
    {
        return loginDto;
        
    }

    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await _authService.Register(dto);

        return Ok(user);
    }
}