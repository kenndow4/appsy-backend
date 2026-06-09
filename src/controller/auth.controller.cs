using Microsoft.AspNetCore.Mvc;
namespace appsy.src.controller;
using appsy.src.dtos;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    [HttpPost("Login")]
    public ActionResult<LoginDto> Login([FromBody]LoginDto loginDto)
    {
        return loginDto;
        
    }

    [HttpPost("Register")]
    public ActionResult<RegisterDto> Register([FromBody]RegisterDto registerDto)
    {
        return registerDto;
    }
}