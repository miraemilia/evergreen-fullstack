using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Shared;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Evergreen.WebAPI.src.Repository;

public class OrderRepository : IOrderRepository
{
    private DbSet<Order> _orders;
    private DatabaseContext _database;
    private DbSet<Product> _products;
    public OrderRepository(DatabaseContext database)
    {
        _orders = database.Orders;
        _database = database;
        _products = database.Products;
    }

    public async Task<Order> CreateOneAsync(Order createItem)
    {
        using ( var transaction = await _database.Database.BeginTransactionAsync())
        {
            try
            {
                foreach(var detail in createItem.OrderDetails)
                {
                    var product = await _products.FirstAsync(u => u.Id == detail.ProductId);
                    if (product.Inventory >= detail.Quantity)
                    {
                        product.Inventory -= detail.Quantity;
                        _products.Update(product);
                        await _database.SaveChangesAsync();
                    }
                    else
                    {
                        throw CustomException.InsufficientInventory($"Could not create order: not enough products in inventory. Product: {product!.Title}, Inventory: {product.Inventory}, Ordered amount: {detail.Quantity}");
                    }
                }
                _orders.Add(createItem);
                await _database.SaveChangesAsync();
                await transaction.CommitAsync();
                return await _orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(od => od.Product).FirstAsync(u => u.Id == createItem.Id);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task<bool> DeleteOneAsync(Order deleteItem)
    {
        _orders.Remove(deleteItem);
        await _database.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(GetAllParams options)
    {
        return await _orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(od => od.Product).OrderByDescending(p => p.UpdatedAt).Skip(options.Offset).Take(options.Limit).ToListAsync();
    }

    public async Task<int> GetCountAsync(GetAllParams options)
    {
        return await _orders.CountAsync();
    }

    public async Task<Order?> GetOneByIdAsync(Guid id)
    {
        return await _orders.Include(o => o.User).Include(o => o.OrderDetails).ThenInclude(od => od.Product).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Order> UpdateOneAsync(Order updateItem)
    {
        _orders.Update(updateItem);
        await _database.SaveChangesAsync();
        return updateItem;
    }
}