using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Evergreen.WebAPI.src.Repository;

public class UserRepository : IUserRepository
{
    private DbSet<User> _users;
    private DatabaseContext _database;

    public UserRepository(DatabaseContext database)
    {
        _users = database.Users;
        _database = database;
    }

    public async Task<User> CreateOneAsync(User user)
    {
        _users.Add(user);
        await _database.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteOneAsync(User user)
    {
        _users.Remove(user);
        await _database.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EmailAvailable(string email)
    {
        var found = await _users.FirstOrDefaultAsync(u => u.Email == email);
        if (found is null)
        {
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<User>> GetAllAsync(GetAllParams options)
    {
        return await _users.Skip(options.Offset).Take(options.Limit).ToListAsync();
    }

    public async Task<int> GetCountAsync(GetAllParams options)
    {
        return await _users.CountAsync();
    }

    public async Task<User?> GetOneByEmailAsync(string email)
    {
        return await _users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetOneByIdAsync(Guid id)
    {
        return await _users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> UpdateOneAsync(User userUpdate)
    {
        _users.Update(userUpdate);
        await _database.SaveChangesAsync();
        return userUpdate;
    }
    
}