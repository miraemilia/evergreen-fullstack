using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IOrderService
{
    Task<OrderPageableReadDTO> GetAllOrdersAsync(GetAllParams options);
    Task<OrderReadDTO> GetOrderByIdAsync(Guid id);
    Task<OrderReadDTO> CreateOrderAsync(OrderCreateDTO newOrder);
    Task<bool> DeleteOrderAsync(Guid id);
    Task<OrderReadDTO> UpdateOrderStatusAsync(Guid id, OrderUpdateDTO updates);
}