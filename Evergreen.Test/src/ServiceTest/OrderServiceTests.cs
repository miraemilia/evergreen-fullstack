using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Service;
using Evergreen.Service.src.Shared;
using Moq;

namespace Evergreen.Test.src;

public class OrderServiceTests
{
    private static IMapper _mapper;

    public OrderServiceTests()
    {
        if (_mapper == null)
        {
            var mappingConfig = new MapperConfiguration(m =>
            {
                m.AddProfile(new MapperProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper; 
        }
    }

    [Fact]
    public async void GetAllAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IOrderRepository>();
        var mapper = new Mock<IMapper>();
        var productRepo = new Mock<IProductRepository>();
        var userRepo = new Mock<IUserRepository>();
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);
        GetAllParams options = new GetAllParams(){Limit = 10, Offset = 0};

        await orderService.GetAllOrdersAsync(options);

        repo.Verify(repo => repo.GetAllAsync(options), Times.Once);
    }

/*     [Theory]
    [ClassData(typeof(GetAllOrdersData))]
    public async void GetAllAsync_ShouldReturnValidResponse(IEnumerable<Order> repoResponse, IEnumerable<OrderReadDTO> expected)
    {
        var repo = new Mock<IOrderRepository>();
        GetAllParams options = new GetAllParams(){Limit = 10, Offset = 0};
        repo.Setup(repo => repo.GetAllAsync(options)).ReturnsAsync(repoResponse);
        var productRepo = new Mock<IProductRepository>();
        var userRepo = new Mock<IUserRepository>();
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);
        
        var response = await orderService.GetAllOrdersAsync(options);

        Assert.Equivalent(expected, response);
    }

    public class GetAllOrdersData : TheoryData<IEnumerable<Order>, OrderPageableReadDTO>
    {
        public GetAllOrdersData()
        {
            Order order1 = new Order(){};
            Order order2 = new Order(){};
            Order order3 = new Order(){};
            IEnumerable<Order> orders = new List<Order>(){order1, order2, order3};
            IEnumerable<OrderReadDTO> readOrders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderReadDTO>>(orders);
            Add(orders, new OrderPageableReadDTO{Items = readOrders, TotalItems = 3, TotalPages = 1});
        }
    } */
 
    [Fact]
    public async void GetOrderById_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IOrderRepository>();
        Order order1 = new Order(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(order1);
        var mapper = new Mock<IMapper>();
        var productRepo = new Mock<IProductRepository>();
        var userRepo = new Mock<IUserRepository>();
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);        

        await orderService.GetOrderByIdAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetOneOrderData))]
    public async void GetOrderById_ShouldReturnValidResponse(Order? repoResponse, OrderReadDTO? expected, Type? exceptionType)
    {
        var repo = new Mock<IOrderRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(repoResponse));
        var productRepo = new Mock<IProductRepository>();
        var userRepo = new Mock<IUserRepository>();
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => orderService.GetOrderByIdAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await orderService.GetOrderByIdAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    
    }

    public class GetOneOrderData : TheoryData<Order?, OrderReadDTO?, Type?>
    {
        public GetOneOrderData()
        {
            Order order1 = new Order(){};
            OrderReadDTO order1Read = new OrderReadDTO(){};
            order1Read.OrderDetails = new List<OrderProductReadDTO>(){};
            Add(order1, order1Read, null);
            Add(null, null, typeof(CustomException));
        }
    }

/*     [Fact]
    public async void CreateOneAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IOrderRepository>();
        var mapper = new Mock<IMapper>();
        var productRepo = new Mock<IProductRepository>();
        productRepo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Product(){});
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User(){});
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);
        OrderCreateDTO dto = new OrderCreateDTO(){};

        await orderService.CreateOrderAsync(dto, It.IsAny<Guid>()); 

        repo.Verify(repo => repo.CreateOneAsync(It.IsAny<Order>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateOrderData))]
    public async void CreateOneAsync_ShouldReturnValidResponse(User? userResponse, Product? productResponse, Order repoResponse, OrderReadDTO expected, Type? exceptionType)
    {
        var repo = new Mock<IOrderRepository>();
        repo.Setup(repo => repo.CreateOneAsync(It.IsAny<Order>())).ReturnsAsync(repoResponse);
        var productRepo = new Mock<IProductRepository>();
        productRepo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(productResponse);
        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(userResponse);
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);
        OrderCreateDTO dto = new OrderCreateDTO(){};

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => orderService.CreateOrderAsync(dto, It.IsAny<Guid>()));
        }
        else
        {
            var response = await orderService.CreateOrderAsync(dto, It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    }

    public class CreateOrderData : TheoryData<User?, Product?, Order?, OrderReadDTO?, Type?>
    {
        public CreateOrderData()
        {
            Order order1 = new Order(){};
            User user = new User(){};
            Product product = new Product(){};
            OrderReadDTO order1Read = new OrderReadDTO(){};
            Add(user, product, order1, order1Read, null);
            Add(null, product, order1, order1Read, typeof(CustomException));
            Add(user, null, order1, order1Read, typeof(CustomException));
        }
    } */

    [Fact]
    public async void DeleteOrderAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IOrderRepository>();
        Order order1 = new Order(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(order1);
        var mapper = new Mock<IMapper>();
        var productRepo = new Mock<IProductRepository>();
        var userRepo = new Mock<IUserRepository>();
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);

        await orderService.DeleteOrderAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.DeleteOneAsync(It.IsAny<Order>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(DeleteOrderData))]
    public async void DeleteOrderAsync_ShouldReturnValidResponse(Order? foundResponse, bool repoResponse, bool? expected, Type? exceptionType)
    {
        var repo = new Mock<IOrderRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundResponse);
        repo.Setup(repo => repo.DeleteOneAsync(It.IsAny<Order>())).ReturnsAsync(repoResponse);
        var productRepo = new Mock<IProductRepository>();
        var userRepo = new Mock<IUserRepository>();
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => orderService.DeleteOrderAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await orderService.DeleteOrderAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    
    }

    public class DeleteOrderData : TheoryData<Order?, bool, bool?, Type?>
    {
        public DeleteOrderData()
        {
            Order order1 = new Order(){OrderStatus = OrderStatus.Pending};
            Order order2 = new Order(){OrderStatus = OrderStatus.Shipped};
            Add(order1, true, true, null);
            Add(order2, true, true, typeof(CustomException));
            Add(null, false, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void UpdateOrderStatusAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IOrderRepository>();
        Order order1 = new Order(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(order1);
        var mapper = new Mock<IMapper>();
        var productRepo = new Mock<IProductRepository>();
        var userRepo = new Mock<IUserRepository>();
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);
        OrderUpdateDTO updates = new OrderUpdateDTO(){OrderStatus = OrderStatus.Delivered};

        await orderService.UpdateOrderStatusAsync(It.IsAny<Guid>(), updates);

        repo.Verify(repo => repo.UpdateOneAsync(It.IsAny<Order>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(UpdateOrderStatusData))]
    public async void UpdateOrderStatusAsync_ShouldReturnValidResponse(OrderUpdateDTO updates, Order? foundResponse, Order repoResponse, OrderReadDTO? expected, Type? exceptionType)
    {
        var repo = new Mock<IOrderRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundResponse);
        repo.Setup(repo => repo.UpdateOneAsync(It.IsAny<Order>())).ReturnsAsync(repoResponse);
        var productRepo = new Mock<IProductRepository>();
        var userRepo = new Mock<IUserRepository>();
        var orderService = new OrderService(repo.Object, _mapper, productRepo.Object, userRepo.Object);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => orderService.UpdateOrderStatusAsync(It.IsAny<Guid>(), updates));
        }
        else
        {
            var response = await orderService.UpdateOrderStatusAsync(It.IsAny<Guid>(), updates);

            Assert.Equivalent(expected, response);
        }
    }

    public class UpdateOrderStatusData : TheoryData<OrderUpdateDTO, Order?, Order?, OrderReadDTO?, Type?>
    {
        public UpdateOrderStatusData()
        {
            OrderUpdateDTO updates1 = new OrderUpdateDTO(){OrderStatus = OrderStatus.Shipped};
            Order order1 = new Order(){};
            OrderReadDTO order1Read = _mapper.Map<Order, OrderReadDTO>(order1);
            Add(updates1, new Order(){}, order1, order1Read, null);
            Add(updates1, null, order1, null, typeof(CustomException));
        }
    }
}