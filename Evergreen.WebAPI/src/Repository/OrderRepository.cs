using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.WebAPI.src.Database;
using Microsoft.EntityFrameworkCore;

namespace Evergreen.WebAPI.src.Repository;

public class OrderRepository : IOrderRepository
{
    private DbSet<Order> _orders;
    private DatabaseContext _database;

    public OrderRepository(DatabaseContext database)
    {
        _orders = database.Orders;
        _database = database;
    }

    public async Task<Order> CreateOneAsync(Order createItem)
    {
        _orders.Add(createItem);
        await _database.SaveChangesAsync();
        return createItem;
    }

    public async Task<bool> DeleteOneAsync(Order deleteItem)
    {
        _orders.Remove(deleteItem);
        await _database.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(GetAllParams options)
    {
        return await _orders.Skip(options.Offset).Take(options.Limit).ToListAsync();
    }

    public async Task<Order?> GetOneByIdAsync(Guid id)
    {
        return await _orders.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Order> UpdateOneAsync(Order updateItem)
    {
        _orders.Update(updateItem);
        await _database.SaveChangesAsync();
        return updateItem;
    }
}