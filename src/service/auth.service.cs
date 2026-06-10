using appsy.src.dtos;
using MongoDB.Driver;

namespace appsy.src.service;

public class AuthService
{
    private readonly IMongoCollection<RegisterDto> _users;

    public AuthService(IMongoDatabase database)
    {
        _users = database.GetCollection<RegisterDto>("users");
    }

    public async Task<RegisterDto> Register(RegisterDto dto)
    {
        var userExists = await _users
            .Find(x => x.Email == dto.Email)
            .FirstOrDefaultAsync();

        if (userExists is not null)
        {
            throw new Exception("El correo ya está registrado");
        }

        await _users.InsertOneAsync(dto);

        return dto;
    }
}