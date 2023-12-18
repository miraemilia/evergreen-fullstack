using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Evergreen.WebAPI.src.Repository;

public class CategoryRepository : ICategoryRepository
{
    private DbSet<Category> _categories;
    private DatabaseContext _database;

    public CategoryRepository(DatabaseContext database)
    {
        _categories = database.Categories;
        _database = database;
    }
    public async Task<Category> CreateOneAsync(Category createItem)
    {
        _categories.Add(createItem);
        await _database.SaveChangesAsync();
        return createItem;
    }

    public async Task<bool> DeleteOneAsync(Category deleteItem)
    {
        _categories.Remove(deleteItem);
        await _database.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Category>> GetAllAsync(GetAllParams options)
    {
        return await _categories.Skip(options.Offset).Take(options.Limit).ToListAsync();
    }

    public async Task<Category?> GetOneByIdAsync(Guid id)
    {
        return await _categories.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Category> UpdateOneAsync(Category updateItem)
    {
        _categories.Update(updateItem);
        await _database.SaveChangesAsync();
        return updateItem;
    }
}