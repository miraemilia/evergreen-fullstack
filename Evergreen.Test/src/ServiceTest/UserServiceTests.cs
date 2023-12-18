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

public class UserServiceTests
{
    private static IMapper _mapper;

    public UserServiceTests()
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
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);
        GetAllParams options = new GetAllParams(){Limit = 10, Offset = 0};

        await userService.GetAllUsersAsync(options);

        repo.Verify(repo => repo.GetAllAsync(options), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetAllUsersData))]
    public async void GetAllAsync_ShouldReturnValidResponse(IEnumerable<User> repoResponse, IEnumerable<UserReadDTO> expected)
    {
        var repo = new Mock<IUserRepository>();
        GetAllParams options = new GetAllParams(){Limit = 10, Offset = 0};
        repo.Setup(repo => repo.GetAllAsync(options)).Returns(Task.FromResult(repoResponse));
        var userService = new UserService(repo.Object, _mapper);
        
        var response = await userService.GetAllUsersAsync(options);

        Assert.Equivalent(expected, response);
    }

    public class GetAllUsersData : TheoryData<IEnumerable<User>, IEnumerable<UserReadDTO>>
    {
        public GetAllUsersData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            User user2 = new User(){Name = "Jane Doe", Email = "jane@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            User user3 = new User(){Name = "Jack Doe", Email = "jack@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            IEnumerable<User> users = new List<User>(){user1, user2, user3};
            Add(users, _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDTO>>(users));
        }
    }

    [Fact]
    public async void GetUserById_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user1);
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.GetUserByIdAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetOneUserData))]
    public async void GetUserById_ShouldReturnValidResponse(User? repoResponse, UserReadDTO? expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(repoResponse));
        var userService = new UserService(repo.Object, _mapper);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => userService.GetUserByIdAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await userService.GetUserByIdAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    
    }

    public class GetOneUserData : TheoryData<User?, UserReadDTO?, Type?>
    {
        public GetOneUserData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(user1, user1Read, null);
            Add(null, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void CreateOneAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.EmailAvailable(It.IsAny<string>())).ReturnsAsync(true);
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);
        UserWithRoleCreateDTO dto = new UserWithRoleCreateDTO(){ Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "placeholder", Role = UserRole.Admin};

        await userService.CreateUserAsync(dto);

        repo.Verify(repo => repo.CreateOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateUserData))]
    public async void CreateOneAsync_ShouldReturnValidResponse(bool emailAvailableResponse, User repoResponse, UserReadDTO expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.CreateOneAsync(It.IsAny<User>())).ReturnsAsync(repoResponse);
        repo.Setup(repo => repo.EmailAvailable(It.IsAny<string>())).ReturnsAsync(emailAvailableResponse);
        var userService = new UserService(repo.Object, _mapper);
        UserWithRoleCreateDTO dto = new UserWithRoleCreateDTO(){ Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "placeholder", Role = UserRole.Admin};

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => userService.CreateUserAsync(dto));
        }
        else
        {
            var response = await userService.CreateUserAsync(dto);

            Assert.Equivalent(expected, response);
        }
    }

    public class CreateUserData : TheoryData<bool, User?, UserReadDTO?, Type?>
    {
        public CreateUserData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(true, user1, user1Read, null);
            Add(false, null, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void DeleteUserAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user1);
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.DeleteUserAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.DeleteOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(DeleteUserData))]
    public async void DeleteUserAsync_ShouldReturnValidResponse(User? foundResponse, bool repoResponse, bool? expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundResponse);
        repo.Setup(repo => repo.DeleteOneAsync(It.IsAny<User>())).ReturnsAsync(repoResponse);
        var userService = new UserService(repo.Object, _mapper);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => userService.DeleteUserAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await userService.DeleteUserAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    
    }

    public class DeleteUserData : TheoryData<User?, bool, bool?, Type?>
    {
        public DeleteUserData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            Add(user1, true, true, null);
            Add(null, false, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void UpdateUser_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user1);
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.UpdateUserRoleAsync(It.IsAny<Guid>(), It.IsAny<UserRole>());

        repo.Verify(repo => repo.UpdateOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(UpdateUserRoleData))]
    public async void UpdateUserRoleAsync_ShouldReturnValidResponse(UserRole newRole, User? foundResponse, User repoResponse, UserReadDTO? expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundResponse);
        repo.Setup(repo => repo.UpdateOneAsync(It.IsAny<User>())).ReturnsAsync(repoResponse);
        var userService = new UserService(repo.Object, _mapper);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => userService.UpdateUserRoleAsync(It.IsAny<Guid>(), newRole));
        }
        else
        {
            var response = await userService.UpdateUserRoleAsync(It.IsAny<Guid>(), newRole);

            Assert.Equivalent(expected, response);
        }
    }

    public class UpdateUserRoleData : TheoryData<UserRole, User?, User, UserReadDTO?, Type?>
    {
        public UpdateUserRoleData()
        {
            User customer = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200", Role = UserRole.Customer};
            User admin = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200", Role = UserRole.Admin};
            UserReadDTO customerRead = _mapper.Map<User, UserReadDTO>(customer);
            UserReadDTO adminRead = _mapper.Map<User, UserReadDTO>(admin);
            Add(UserRole.Customer, new User(){}, customer, customerRead, null);
            Add(UserRole.Admin, new User(){}, admin, adminRead, null);
            Add(UserRole.Customer, null, customer, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void EmailAvailableAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.EmailAvailableAsync(It.IsAny<string>());

        repo.Verify(repo => repo.EmailAvailable(It.IsAny<string>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(EmailAvailableData))]
    public async void EmailAvailableAsync_ShouldReturnValidResponse(bool repoResponse, bool expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.EmailAvailable(It.IsAny<string>())).ReturnsAsync(repoResponse);
        var userService = new UserService(repo.Object, _mapper);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => userService.EmailAvailableAsync(It.IsAny<string>()));
        }
        else
        {
            var response = await userService.EmailAvailableAsync(It.IsAny<string>());

            Assert.Equivalent(expected, response);
        }
    }

    public class EmailAvailableData : TheoryData<bool, bool, Type?>
    {
        public EmailAvailableData()
        {
            Add(true, true, null);
            Add(false, false, null);
        }
    }

}