using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IUserService
{
    IEnumerable<UserReadDTO> GetAllUsers(GetAllParams options);
    UserReadDTO GetUserById(Guid id);
    UserReadDTO CreateUser(UserCreateDTO user);
    bool DeleteUser(Guid id);
    UserReadDTO UpdateUser(UserUpdateDTO update);
    UserReadDTO ChangeUserRole(UserRoleUpdateDTO update);
    string Login(LoginDTO loginDTO);
    bool EmailAvailable(string email);
}