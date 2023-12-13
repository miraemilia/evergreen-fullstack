using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;
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

    //fails
    [Fact]
    public async void CreateOneAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.CreateUserAsync(It.IsAny<UserCreateDTO>());

        repo.Verify(repo => repo.CreateOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateUserData))]
    public async void CreateOneAsync_ShouldReturnValidResponse(User repoResponse, UserReadDTO expected)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.CreateOneAsync(It.IsAny<User>())).Returns(Task.FromResult(repoResponse));
        var userService = new UserService(repo.Object, _mapper);
        
        var response = await userService.CreateUserAsync(It.IsAny<UserCreateDTO>());

        Assert.Equivalent(expected, response);
    }

    public class CreateUserData : TheoryData<User, UserReadDTO>
    {
        public CreateUserData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(user1, user1Read);
        }
    }

    //fails
    [Fact]
    public async void GetUserById_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.GetUserByIdAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetOneUserData))]
    public async void GetUserById_ShouldReturnValidResponse(User? repoResponse, UserReadDTO expected)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(repoResponse));
        var userService = new UserService(repo.Object, _mapper);
        
        var response = await userService.GetUserByIdAsync(It.IsAny<Guid>());

        Assert.Equivalent(expected, response);
    }

    public class GetOneUserData : TheoryData<User, UserReadDTO>
    {
        public GetOneUserData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(user1, user1Read);
        }
    }

    [Fact]
    public async void DeleteUser_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.DeleteUserAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.DeleteOneAsync(It.IsAny<Guid>()), Times.Once);
    }

    //fails
    [Fact]
    public async void UpdateUser_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.UpdateUserAsync(It.IsAny<Guid>(), It.IsAny<UserUpdateDTO>());

        repo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
        repo.Verify(repo => repo.UpdateOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async void EmailAvailable_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        await userService.EmailAvailableAsync(It.IsAny<string>());

        repo.Verify(repo => repo.GetOneByEmailAsync(It.IsAny<string>()), Times.Once);
    }

}