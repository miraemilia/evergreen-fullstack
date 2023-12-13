using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IUserService
{
    Task<IEnumerable<UserReadDTO>> GetAllUsersAsync(GetAllParams options);
    Task<UserReadDTO> GetUserByIdAsync(Guid id);
    Task<UserReadDTO> CreateUserAsync(UserCreateDTO user);
    Task<bool> DeleteUserAsync(Guid id);
    Task<UserReadDTO> UpdateUserAsync(Guid id, UserUpdateDTO update);
    Task<bool> EmailAvailableAsync(string email);
}