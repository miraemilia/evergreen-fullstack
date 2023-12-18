using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Evergreen.WebAPI.src.Repository;

public class ProductRepository : IProductRepository
{
    private DbSet<Product> _products;
    private DatabaseContext _database;

    public ProductRepository(DatabaseContext database)
    {
        _products = database.Products;
        _database = database;
    }
    public async Task<Product> CreateOneAsync(Product createItem)
    {
        _products.Add(createItem);
        await _database.SaveChangesAsync();
        return createItem;
    }

    public async Task<bool> DeleteOneAsync(Product deleteItem)
    {
        _products.Remove(deleteItem);
        await _database.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(GetAllParams options)
    {
        return await _products.Skip(options.Offset).Take(options.Limit).ToListAsync();
    }

    public async Task<Product?> GetOneByIdAsync(Guid id)
    {
        return await _products.FirstOrDefaultAsync(u => u.Id == id);

/*         Join(_database.ProductDetails,
            product => product.Id,
            details => details.ProductId,
            (product, details) => new {
                
            }) */
    }

    public async Task<Product> UpdateOneAsync(Product updateItem)
    {
        _products.Update(updateItem);
        await _database.SaveChangesAsync();
        return updateItem;
    }
}