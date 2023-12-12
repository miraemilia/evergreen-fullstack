using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
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

/*     [Theory]
    [ClassData(typeof(GetAllUsersData))]
    public void GetAll_ShouldReturnValidResponse(IEnumerable<UserReadDTO> repoResponse, IEnumerable<UserReadDTO> expected)
    {
        var repo = new Mock<IUserRepository>();
        GetAllParams options = new GetAllParams(10, 0);
        repo.Setup(repo => repo.GetAllUsers(options).Returns(repoResponse));
        var userService = new UserService(repo.Object, _mapper);
        
        var response = userService.GetAllUsers(options);

        Assert.Equivalent(expected, response);
    }

    public class GetAllUsersData : TheoryData<IEnumerable<User>, IEnumerable<UserReadDTO>>
    {
        public GetAllUsersData()
        {
            //User user1 = new User( Name = "John Doe", Email = "john@example.com");
        }
    } */
}