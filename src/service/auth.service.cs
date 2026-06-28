using appsy.src.dtos;
using appsy.src.repository;
using BCrypt.Net;
namespace appsy.src.service;

public class AuthService
{
private readonly AuthRepository _repository;
private readonly JwtService _jwtService;
public AuthService (AuthRepository repository, JwtService jwtService)
    {
        _repository =repository;
         _jwtService = jwtService;
    }
   
  
    //REGISTER 
    public async Task<RegisterDto> Register(RegisterDto dtoRegister)
    {
        var userExist = await _repository.GetByEmail(dtoRegister.Email);
      if (userExist is not null)
{
    throw new Exception("El correo ya está registrado");
}   dtoRegister.Password = BCrypt.Net.BCrypt.HashPassword(dtoRegister.Password);
    await _repository.Create(dtoRegister);

        return dtoRegister;
    }

    //LOGIN    
      public async Task<AuthResponseDto> Login( LoginDto  dto)
    {
        var user = await _repository.GetByEmail(dto.Email);
         if (user == null)
        throw new Exception("Correo o contraseña incorrectos");
      bool isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password , user.Password);
    if (!isValidPassword)
{
    throw new Exception("Correo o contraseña incorrectos");
}
    

    var token = _jwtService.GenerateToken(user.Id, user.Email);
return new AuthResponseDto
{
    Token = token,
    User = new UserDto
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email
    }
};
    }
    
    
    }
  