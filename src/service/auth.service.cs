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
   

   public async Task<UserDto> GetUserById(string id)
    {
        var user = await _repository.GetById(id);
        if (user == null)
        {
            throw new Exception("Usuario no encontrado");
        }

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Avatar = user.Avatar
        };
    }
  
    //REGISTER 
    public async Task<UserDto> Register(RegisterDto dtoRegister)
    {
        var userExist = await _repository.GetByEmail(dtoRegister.Email);
      if (userExist is not null)
{
    throw new Exception("El correo ya está registrado");
}   dtoRegister.Password = BCrypt.Net.BCrypt.HashPassword(dtoRegister.Password);
     Random random = new Random();
        string randomColor = random.Next(0x1000000).ToString("X6");
        string randomSeed = Guid.NewGuid().ToString();
        string avatarUrl = $"https://api.dicebear.com/9.x/adventurer-neutral/svg?seed={randomSeed}&backgroundColor={randomColor}";

    dtoRegister.Avatar = avatarUrl;
    await _repository.Create(dtoRegister);

        return new UserDto
        {
            Id = dtoRegister.Id,
            Name = dtoRegister.Name,
            Email = dtoRegister.Email,
            Avatar = dtoRegister.Avatar
        };
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
        Avatar = user.Avatar,
        Email = user.Email
    }
};
    }
    
    
    }
  