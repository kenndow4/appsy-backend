namespace appsy.src.repository;
using appsy.src.dtos;
using MongoDB.Driver;


public class AuthRepository
{
    
private readonly IMongoCollection<RegisterDto> _users;

public  AuthRepository( IMongoDatabase database)
    {
        _users = database.GetCollection<RegisterDto>("users");
    }

public async Task<RegisterDto?> GetByEmail(string email)
    {
      return await _users.Find(x => x.Email == email).FirstOrDefaultAsync();


    }

public async Task<RegisterDto?> Create(RegisterDto dto)
    {
       await _users.InsertOneAsync(dto);
        return dto;
    }
}