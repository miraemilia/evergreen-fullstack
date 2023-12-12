using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

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
    }

    public User AddUser(User user)
    {
        _users.Add(user);
        _database.SaveChanges();
        return user;
    }

    public User ChangeUserRole(Guid id, UserRole role)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetAllUsers(GetAllParams options)
    {
        return _users.Skip(options.Offset).Take(options.Limit);
    }

    public User GetUser(Guid id)
    {
        return (User)_users.Where(u => u.Id.Equals(id));
    }

    public User UpdateUser(Guid id, string Name)
    {
        throw new NotImplementedException();
    }
}