using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Shared;
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
        if (options.Id == null || options.Id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        {
            return await _products.Include("Category").Include("ProductDetails").Include("ProductImages").Where(p => p.Title.ToLower().Contains(options.Search.ToLower())).Skip(options.Offset).Take(options.Limit).ToListAsync(); 
        }
        return await _products.Include("Category").Include("ProductDetails").Include("ProductImages").Where(p => p.Category.Id == options.Id && p.Title.ToLower().Contains(options.Search.ToLower())).Skip(options.Offset).Take(options.Limit).ToListAsync();
    }

    public async Task<int> GetCountAsync(GetAllParams options)
    {
        if (options.Id == null || options.Id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
        {
            return await _products.Include("Category").Include("ProductDetails").Include("ProductImages").Where(p => p.Title.ToLower().Contains(options.Search.ToLower())).CountAsync(); 
        }
        return await _products.Include("Category").Include("ProductDetails").Include("ProductImages").Where(p => p.Category.Id == options.Id && p.Title.ToLower().Contains(options.Search.ToLower())).CountAsync();
    }

    public async Task<Product?> GetOneByIdAsync(Guid id)
    {
        return await _products.Include("Category").Include("ProductDetails").Include("ProductImages").FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<Image>> GetProductImages(Guid productId)
    {
        var product = await _products.Include("ProductImages").FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null)
        {
            throw CustomException.NotFoundException("Product not found");
        }
        return product.ProductImages;
    }

    public async Task<Product> UpdateOneAsync(Product updateItem)
    {
        _products.Update(updateItem);
        await _database.SaveChangesAsync();
        return updateItem;
    }

    public async Task<MaxMinPrice> GetMaxMinPrice()
    {
        var max = await _products.MaxAsync(p => p.Price);
        var min = await _products.MinAsync(p => p.Price);
        return new MaxMinPrice(){Max = max, Min = min};
    }
}