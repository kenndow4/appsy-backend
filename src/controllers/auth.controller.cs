using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    [HttpPost("Login")]
    public LoginDto Login(LoginDto loginDto)
    {
        return loginDto;
        
    }

    [HttpPost("Register")]
    public RegisterDto Register(RegisterDto registerDto)
    {
        return registerDto;
    }
}