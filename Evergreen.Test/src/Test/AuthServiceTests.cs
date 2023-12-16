using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
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
    public async void GetProfileAsync_ShouldInvokeRepoMethod()
    {
        var repo = new Mock<IUserRepository>();
        User user = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
        repo.Setup(repo => repo.GetOneByIdAsync(It.IsAny<Guid>())).ReturnsAsync(user);
        var mapper = new Mock<IMapper>();
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);

        await authService.GetProfileAsync(It.IsAny<Guid>());

        repo.Verify(repo => repo.GetOneByIdAsync(It.IsAny<Guid>()), Times.Once);
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
    public async void CreateProfileAsync_ShouldReturnValidResponse(bool emailAvailableResponse, UserCreateDTO dto, User repoResponse, UserReadDTO expected)
    {
        var repo = new Mock<IUserRepository>();
        repo.Setup(repo => repo.CreateOneAsync(It.IsAny<User>())).Returns(Task.FromResult(repoResponse));
        repo.Setup(repo => repo.EmailAvailable(It.IsAny<string>())).Returns(Task.FromResult(emailAvailableResponse));
        var tokenService = new Mock<ITokenService>();
        var authService = new AuthService(repo.Object, _mapper, tokenService.Object);
        var userServiceMock = new Mock<IUserService>();
        
        var response = await authService.CreateProfileAsync(dto);

        Assert.Equivalent(expected, response);
    }

    public class CreateProfileData : TheoryData<bool, UserCreateDTO, User, UserReadDTO>
    {
        public CreateProfileData()
        {
            UserCreateDTO dto = new UserCreateDTO(){ Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            User user1 = new User(){Name = "John Doe", Email = "john@example.com", Password = "12345", Avatar = "https://picsum.photos/200"};
            UserReadDTO user1Read = _mapper.Map<User, UserReadDTO>(user1);
            Add(true, dto, user1, user1Read);
        }
    }
}