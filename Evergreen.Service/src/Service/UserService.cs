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
        var result = _userRepo.AddUser(_mapper.Map<UserCreateDTO, User>(user));
        return _mapper.Map<User, UserReadDTO>(result);
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
        var result = _userRepo.GetUserById(id);
        return _mapper.Map<User, UserReadDTO>(result);
    }

    public string Login(LoginDTO loginDTO)
    {
        User user = _userRepo.GetUserByCredentials(loginDTO.Email, loginDTO.Password);
        Console.WriteLine(user);
        return _userRepo.GenerateToken(user);
    }

    public UserReadDTO UpdateUser(UserUpdateDTO update)
    {
        throw new NotImplementedException();
    }
}