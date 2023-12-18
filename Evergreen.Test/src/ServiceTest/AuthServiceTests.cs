using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Service;
using Evergreen.Service.src.Shared;
using Moq;

namespace Evergreen.Test.src;

public class AuthServiceTests
{
    private static IMapper _mapper;

    public AuthServiceTests()
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
    public async void Login_ShouldInvokeTokenService()
    {
        var repo = new Mock<IUserRepository>();
        User user = new User(){};
        repo.Setup(repo => repo.GetOneByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(ts => ts.GenerateToken(It.IsAny<User>())).Returns("token");
        //var passwordService = new Mock<IPasswordService>();
        //passwordService.Setup(ps => ps.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>())).Returns(true);
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        var loginParams = new LoginParams(){Email = "user@mail.com", Password = "password"};

        await authService.Login(It.IsAny<LoginParams>());

        tokenService.Verify(service => service.GenerateToken(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(LoginData))]
    public async void Login_ShouldReturnValidResponse(User? foundUser, string tokenServiceResponse, string expected, Type exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetOneByEmailAsync(It.IsAny<string>())).ReturnsAsync(foundUser);
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        tokenService.Setup(ts => ts.GenerateToken(It.IsAny<User>())).Returns(tokenServiceResponse);
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        //var passwordService = new Mock<IPasswordService>();
        //passwordService.Setup(ps => ps.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<byte[]>())).Returns(true);
        var loginParams = new LoginParams(){Email = "user@mail.com", Password = "password"};

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => authService.Login(loginParams));
        }
        else
        {
            var response = await authService.Login(loginParams);

            Assert.Equivalent(expected, response);
        }
    }

    public class LoginData : TheoryData<User?, string?, string?, Type?>
    {
        public LoginData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            Add(user1, "generatedtoken", "generatedtoken", null);
            Add(null, null, null, typeof(CustomException));
            Add(user1, null, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void GetProfileAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        User user = new User(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);

        await authService.GetProfileAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(GetProfileData))]
    public async void GetProfileAsync_ShouldReturnValidResponse(User? repoResponse, UserReadDTO expected, Type exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(repoResponse));
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => authService.GetProfileAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await authService.GetProfileAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
    }

    public class GetProfileData : TheoryData<User?, UserReadDTO?, Type?>
    {
        public GetProfileData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(user1, user1Read, null);
            Add(null, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void CreateProfileAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.EmailAvailable(It.IsAny<string>())).ReturnsAsync(true);
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        UserCreateDTO dto = new UserCreateDTO(){ Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "placeholder"};

        await authService.CreateProfileAsync(dto);

        repo.Verify(repo => repo.CreateOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(CreateProfileData))]
    public async void CreateProfileAsync_ShouldReturnValidResponse(bool emailAvailableResponse, UserCreateDTO dto, User repoResponse, UserReadDTO expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.CreateOneAsync(It.IsAny<User>())).Returns(Task.FromResult(repoResponse));
        repo.Setup(repo => repo.EmailAvailable(It.IsAny<string>())).Returns(Task.FromResult(emailAvailableResponse));
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        var userServiceMock = new Mock<IUserService>();

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => authService.CreateProfileAsync(dto));
        }
        else
        {
            var response = await authService.CreateProfileAsync(dto);

            Assert.Equivalent(expected, response);
        }
        
    }

    public class CreateProfileData : TheoryData<bool, UserCreateDTO, User?, UserReadDTO?, Type?>
    {
        public CreateProfileData()
        {
            UserCreateDTO dto = new UserCreateDTO(){ Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(true, dto, user1, user1Read, null);
            Add(false, dto, null, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void UpdateProfileAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        User user = new User(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
        repo.Setup(repo => repo.EmailAvailable(It.IsAny<string>())).ReturnsAsync(true);
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        UserUpdateDTO dto = new UserUpdateDTO(){ Name = "John Doe", Email = "john@example.com", Avatar = "placeholder"};

        await authService.UpdateProfileAsync(It.IsAny<Guid>(), dto);

        repo.Verify(repo => repo.UpdateOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(UpdateProfileData))]
    public async void UpdateProfileAsync_ShouldReturnValidResponse(UserUpdateDTO dto, User? foundUser, bool emailResponse, User repoResponse, UserReadDTO expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        User user = new User(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundUser);
        repo.Setup(repo => repo.UpdateOneAsync(It.IsAny<User>())).Returns(Task.FromResult(repoResponse));
        repo.Setup(repo => repo.EmailAvailable(It.IsAny<string>())).Returns(Task.FromResult(emailResponse));
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        var userServiceMock = new Mock<IUserService>();

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => authService.UpdateProfileAsync(It.IsAny<Guid>(), dto));
        }
        else
        {
            var response = await authService.UpdateProfileAsync(It.IsAny<Guid>(), dto);

            Assert.Equivalent(expected, response);
        }
        
    }

    public class UpdateProfileData : TheoryData<UserUpdateDTO, User?, bool, User, UserReadDTO?, Type?>
    {
        public UpdateProfileData()
        {
            UserUpdateDTO dto1 = new UserUpdateDTO(){ Name = "John Doe", Email = "john@example.com", Avatar = "https://picsum.photos/200"};
            UserUpdateDTO dto2 = new UserUpdateDTO(){ Email = "john@example.com"};
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(dto1, user1, true, user1, user1Read, null);
            Add(dto2, user1, true, user1, user1Read, null);
            Add(dto2, user1, false, user1, null, typeof(CustomException));
            Add(dto2, null, true, user1, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void ChangePasswordAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new User(){});
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);

        await authService.ChangePasswordAsync(It.IsAny<Guid>(), "newpassword");

        repo.Verify(repo => repo.UpdateOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(ChangePasswordData))]
    public async void ChangePasswordAsync_ShouldReturnValidResponse(User? foundResponse, User repoResponse, UserReadDTO expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundResponse);
        repo.Setup(repo => repo.UpdateOneAsync(It.IsAny<User>())).Returns(Task.FromResult(repoResponse));
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        var userServiceMock = new Mock<IUserService>();

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => authService.ChangePasswordAsync(It.IsAny<Guid>(), "newpassword"));
        }
        else
        {
            var response = await authService.ChangePasswordAsync(It.IsAny<Guid>(), "newpassword");

            Assert.Equivalent(expected, response);
        }
        
    }

    public class ChangePasswordData : TheoryData<User?, User, UserReadDTO?, Type?>
    {
        public ChangePasswordData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(user1, user1, user1Read, null);
            Add(null, user1, null, typeof(CustomException));
        }
    }

    [Fact]
    public async void DeleteProfileAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        User user = new User(){};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);

        await authService.DeleteProfileAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.DeleteOneAsync(It.IsAny<User>()), Times.Once);
    }

    [Theory]
    [ClassData(typeof(DeleteProfileData))]
    public async void DeleteProfileAsync_ShouldReturnValidResponse(User? foundUser, bool repoResponse, bool expected, Type? exceptionType)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.DeleteOneAsync(It.IsAny<User>())).ReturnsAsync(repoResponse);
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(foundUser);
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        var userServiceMock = new Mock<IUserService>();

        if (exceptionType is not null)
        {
            await Assert.ThrowsAsync(exceptionType, () => authService.DeleteProfileAsync(It.IsAny<Guid>()));
        }
        else
        {
            var response = await authService.DeleteProfileAsync(It.IsAny<Guid>());

            Assert.Equivalent(expected, response);
        }
        
    }

    public class DeleteProfileData : TheoryData<User?, bool, bool, Type?>
    {
        public DeleteProfileData()
        {
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            Add(user1, true, true, null);
            Add(null, false, false, typeof(CustomException));
        }
    }

     
}