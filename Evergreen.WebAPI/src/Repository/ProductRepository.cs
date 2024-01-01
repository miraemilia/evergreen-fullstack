using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
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
        var query = _products.Include(p => p.Category).Include(p => p.ProductDetails).Include(p => p.ProductImages).Where(p => p.Title.ToLower().Contains(options.Search.ToLower())).AsQueryable();
        if (options.Id.HasValue && options.Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
        {
            query = query.Where(p => p.Category.Id == options.Id);
        }
        if (options.SortCriterion == SortCriterion.Price && options.SortOrder == SortOrder.Asc)
        {
            query = query.OrderBy(p => p.Price);
        }
        if (options.SortCriterion == SortCriterion.CreatedAt && options.SortOrder == SortOrder.Asc)
        {
            query = query.OrderBy(p => p.CreatedAt);
        }
        if (options.SortCriterion == SortCriterion.Price && options.SortOrder == SortOrder.Desc)
        {
            query = query.OrderByDescending(p => p.Price);
        }
        if (options.SortCriterion == SortCriterion.CreatedAt && options.SortOrder == SortOrder.Desc)
        {
            query = query.OrderByDescending(p => p.CreatedAt);
        }
        if (options.PriceMin.HasValue)
        {
            query = query.Where(p => p.Price >= Convert.ToDecimal(options.PriceMin));
        }
        if (options.PriceMax.HasValue)
        {
            query = query.Where(p => p.Price <= Convert.ToDecimal(options.PriceMax));
        }
        query = query.Skip(options.Offset).Take(options.Limit);
        return await query.ToListAsync();
    }

    public async Task<int> GetCountAsync(GetAllParams options)
    {
        var query = _products.Include(p => p.Category).Include(p => p.ProductDetails).Include(p => p.ProductImages).Where(p => p.Title.ToLower().Contains(options.Search.ToLower())).AsQueryable();
        if (options.Id.HasValue && options.Id != Guid.Parse("00000000-0000-0000-0000-000000000000"))
        {
            query = query.Where(p => p.Category.Id == options.Id);
        }
        if (options.SortCriterion == SortCriterion.Price && options.SortOrder == SortOrder.Asc)
        {
            query = query.OrderBy(p => p.Price);
        }
        if (options.SortCriterion == SortCriterion.CreatedAt && options.SortOrder == SortOrder.Asc)
        {
            query = query.OrderBy(p => p.CreatedAt);
        }
        if (options.SortCriterion == SortCriterion.Price && options.SortOrder == SortOrder.Desc)
        {
            query = query.OrderByDescending(p => p.Price);
        }
        if (options.SortCriterion == SortCriterion.CreatedAt && options.SortOrder == SortOrder.Desc)
        {
            query = query.OrderByDescending(p => p.CreatedAt);
        }
        if (options.PriceMin.HasValue)
        {
            query = query.Where(p => p.Price >= Convert.ToDecimal(options.PriceMin));
        }
        if (options.PriceMax.HasValue)
        {
            query = query.Where(p => p.Price <= Convert.ToDecimal(options.PriceMax));
        }
        return await query.CountAsync();
    }

    public async Task<Product?> GetOneByIdAsync(Guid id)
    {
        return await _products.Include(p => p.Category).Include(p => p.ProductDetails).Include(p => p.ProductImages).FirstOrDefaultAsync(u => u.Id == id);
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

    public async Task<IEnumerable<Image>> GetProductImages(Guid productId)
    {

        var product = await _products.Include("ProductImages").FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null)
        {
            throw CustomException.NotFoundException("Product not found");
        }
        return product.ProductImages;
    }
}