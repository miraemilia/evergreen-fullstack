using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Shared;

namespace Evergreen.Service.src.Service;

public class OrderService : IOrderService
{
    private IOrderRepository _orderRepo;
    private IMapper _mapper;
    private IProductRepository _productRepo;
    private IUserRepository _userRepo;

    public OrderService(IOrderRepository orderRepo, IMapper mapper, IProductRepository productRepo, IUserRepository userRepo)
    {
        _orderRepo = orderRepo;
        _mapper = mapper;
        _productRepo = productRepo;
        _userRepo = userRepo;
    }

    public async Task<OrderReadDTO> CreateOrderAsync(OrderCreateDTO newOrder)
    {
        var user = await _userRepo.GetOneByIdAsync(newOrder.UserId);
        if (user is null)
        {
            throw CustomException.NotFoundException("User not found");
        }
        foreach (OrderProductCreateDTO dto in newOrder.OrderDetails)
        {
            var product = await _productRepo.GetOneByIdAsync(dto.ProductId);
            if (product is null)
            {
                throw CustomException.NotFoundException("Product not found");
            }
        }
        var order = _mapper.Map<OrderCreateDTO, Order>(newOrder);
        var result = await _orderRepo.CreateOneAsync(order);
        return _mapper.Map<Order, OrderReadDTO>(result);
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        var orderToDelete = await _orderRepo.GetOneByIdAsync(id);
        if (orderToDelete == null)
        {
            throw CustomException.NotFoundException("Order not found");
        }
        if (orderToDelete.OrderStatus != OrderStatus.Pending)
        {
            throw CustomException.OrderEditingNotAllowed("A processed order cannot be deleted.");
        }
        return await _orderRepo.DeleteOneAsync(orderToDelete);
    }

    public async Task<OrderPageableReadDTO> GetAllOrdersAsync(GetAllParams options)
    {
        var result = await _orderRepo.GetAllAsync(options);
        var total = await _orderRepo.GetCountAsync(options);
        var foundOrders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderReadDTO>>(result);
        int pages = (total + options.Limit -1)/options.Limit;
        var response = new OrderPageableReadDTO(){Items = foundOrders, TotalItems = total, Pages = pages};
        return response;
    }

    public async Task<OrderReadDTO> GetOrderByIdAsync(Guid id)
    {
        var result = await _orderRepo.GetOneByIdAsync(id);
        if (result != null)
        {
            return _mapper.Map<Order, OrderReadDTO>(result);
        }
        else
        {
            throw CustomException.NotFoundException("Order not found");
        }
    }

    public async Task<OrderReadDTO> UpdateOrderStatusAsync(Guid id, OrderUpdateDTO updates)
    {
        var orderToUpdate = await _orderRepo.GetOneByIdAsync(id);
        if (orderToUpdate != null)
        {
            var updatedOrder = _mapper.Map(updates, orderToUpdate);
            var updated = await _orderRepo.UpdateOneAsync(updatedOrder);
            return _mapper.Map<Order, OrderReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("Order not found");
        }
    }
}