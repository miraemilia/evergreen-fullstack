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
    public void GetAll_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);
        GetAllParams options = new GetAllParams(){Limit = 10, Offset = 0};

        userService.GetAllUsers(options);

        repo.Verify(repo => repo.GetAllUsers(options), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetAllUsersData))]
    public void GetAll_ShouldReturnValidResponse(IEnumerable<User> repoResponse, IEnumerable<UserReadDTO> expected)
    {
        var repo = new Mock<IUserRepository>();
        GetAllParams options = new GetAllParams(){Limit = 10, Offset = 0};
        repo.Setup(repo => repo.GetAllUsers(options)).Returns(repoResponse);
        var userService = new UserService(repo.Object, _mapper);
        
        var response = userService.GetAllUsers(options);

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
    public void CreateUser_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        userService.CreateUser(It.IsAny<UserCreateDTO>());

        repo.Verify(repo => repo.AddUser(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateUserData))]
    public void CreateUser_ShouldReturnValidResponse(User repoResponse, UserReadDTO expected)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.AddUser(It.IsAny<User>())).Returns(repoResponse);
        var userService = new UserService(repo.Object, _mapper);
        
        var response = userService.CreateUser(It.IsAny<UserCreateDTO>());

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

    [Fact]
    public void GetUserById_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        userService.GetUserById(It.IsAny<Guid>());

        repo.Verify(repo => repo.GetUserById(It.IsAny<Guid>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetOneUserData))]
    public void GetUserById_ShouldReturnValidResponse(User repoResponse, UserReadDTO expected)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetUserById(It.IsAny<Guid>())).Returns(repoResponse);
        var userService = new UserService(repo.Object, _mapper);
        
        var response = userService.GetUserById(It.IsAny<Guid>());

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

/*     [Fact]
    public void DeleteUser_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        userService.DeleteUser(It.IsAny<Guid>());

        repo.Verify(repo => repo.DeleteUser(It.IsAny<Guid>()), Times.Once);
    } */

/*     [Fact]
    public void UpdateUser_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        userService.UpdateUser(It.IsAny<UserUpdateDTO>());

        repo.Verify(repo => repo.UpdateUser(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
    } */

/*     [Fact]
    public void ChangeUserRole_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        userService.ChangeUserRole(It.IsAny<UserRoleUpdateDTO>());

        repo.Verify(repo => repo.ChangeUserRole(It.IsAny<Guid>(), It.IsAny<UserRole>()), Times.Once);
    } */

/*     [Fact]
    public void EmailAvailable_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        userService.EmailAvailable(It.IsAny<string>());

        repo.Verify(repo => repo.EmailAvailable(It.IsAny<string>()), Times.Once);
    } */

/*     [Fact]
    public void Login_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        var mapper = new Mock<IMapper>();
        var userService = new UserService(repo.Object, _mapper);

        userService.Login(It.IsAny<LoginDTO>());

        repo.Verify(repo => repo.GetUserByCredentials(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        //repo.Verify(repo => repo.GenerateToken(It.IsAny<User>()), Times.Once);
    } */
}