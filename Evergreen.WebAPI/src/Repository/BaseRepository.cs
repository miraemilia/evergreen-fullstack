using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Evergreen.WebAPI.src.Repository;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> _data;
    protected readonly DatabaseContext _database;

    public BaseRepository(DatabaseContext database)
    {
        _database = database;
        _data = _database.Set<T>();
    }

    public virtual async Task<T> CreateOneAsync(T createItem)
    {
        _data.Add(createItem);
        await _database.SaveChangesAsync();
        return createItem;
    }

    public virtual async Task<bool> DeleteOneAsync(T deleteItem)
    {
        _data.Remove(deleteItem);
        await _database.SaveChangesAsync();
        return true;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(GetAllParams options)
    {
        return await _data.Skip(options.Offset).Take(options.Limit).ToListAsync();
    }

    public async Task<int> GetCountAsync(GetAllParams options)
    {
        return await _data.CountAsync();
    }

    public virtual async Task<T?> GetOneByIdAsync(Guid id)
    {
        return await _data.FirstOrDefaultAsync(u => u.Id == id);
    }

    public virtual async Task<T> UpdateOneAsync(T updateItem)
    {
        _data.Update(updateItem);
        await _database.SaveChangesAsync();
        return updateItem;
    }
}