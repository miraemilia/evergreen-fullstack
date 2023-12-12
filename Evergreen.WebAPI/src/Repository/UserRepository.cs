using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Evergreen.WebAPI.src.Repository;

public class UserRepository : IUserRepository
{
    private DbSet<User> _users;
    private DatabaseContext _database;
    private IConfiguration _config;

    public UserRepository(DatabaseContext database, IConfiguration config)
    {
        _users = database.Users;
        _database = database;
        _config = config;
    }

    public User AddUser(User user)
    {
        _users.Add(user);
        _database.SaveChanges();
        return user;
    }

    public bool DeleteUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool EmailAvailable(string email)
    {
        throw new NotImplementedException();
    }

    public string GenerateToken(User user)
    {
        var issuer = _config.GetSection("Jwt:Issuer").Value;
        var claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var audience = _config.GetSection("Jwt:Audience").Value;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));
        var signingKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Expires = DateTime.Now.AddDays(2),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = signingKey
        };
        Console.WriteLine("about to create token..."); //is logged, bug after this point
        var token = tokenHandler.CreateToken(descriptor);
        Console.WriteLine("after creating token"); //not logged
        return token.ToString()!;
    }

    public IEnumerable<User> GetAllUsers(GetAllParams options)
    {
        return _users.Skip(options.Offset).Take(options.Limit);
    }

    public User GetUserByCredentials(string email, string password)
    {
        return _users.Single(u => u.Email == email && u.Password == password);
    }

    public User GetUserById(Guid id)
    {
        return _users.Single(u => u.Id == id);
    }

    public User UpdateUser(Guid id, User user)
    {
        throw new NotImplementedException();
    }
}