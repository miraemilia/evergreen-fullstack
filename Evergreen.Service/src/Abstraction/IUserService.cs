using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IUserService
{
    Task<IEnumerable<UserReadDTO>> GetAllUsersAsync(GetAllParams options);
    Task<UserReadDTO> GetUserByIdAsync(Guid id);
    Task<UserReadDTO> CreateUserAsync(UserWithRoleCreateDTO newUser);
    Task<bool> DeleteUserAsync(Guid id);
    Task<UserReadDTO> UpdateUserRoleAsync(Guid id, UserRole newRole);
    Task<bool> EmailAvailableAsync(string email);
}