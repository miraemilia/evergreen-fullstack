using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Service;

public class UserService : IUserService
{
    private IUserRepository _userRepo;
    private IMapper _mapper;

    public UserService(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    public UserReadDTO ChangeUserRole(UserRoleUpdateDTO update)
    {
        throw new NotImplementedException();
    }

    public UserReadDTO CreateUser(UserCreateDTO user)
    {
        throw new NotImplementedException();
    }

    public bool DeleteUser(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool EmailAvailable(string email)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserReadDTO> GetAllUsers(GetAllParams options)
    {
        var result = _userRepo.GetAllUsers(options);
        return _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDTO>>(result);
    }

    public UserReadDTO GetUserById(Guid id)
    {
        throw new NotImplementedException();
    }

    public string Login(LoginDTO loginDTO)
    {
        throw new NotImplementedException();
    }

    public UserReadDTO UpdateUser(UserUpdateDTO update)
    {
        throw new NotImplementedException();
    }
}