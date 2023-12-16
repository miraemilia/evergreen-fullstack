using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IAuthService
{
        Task<string> Login(LoginParams loginParams);
        Task<UserReadDTO> GetProfileAsync(Guid id);
        Task<UserReadDTO> CreateProfileAsync(UserCreateDTO newUser);
        Task<UserReadDTO> UpdateProfileAsync(Guid id, UserUpdateDTO update);
        Task<UserReadDTO> ChangePasswordAsync(Guid id, string password);
        Task<bool> DeleteProfileAsync(Guid id);
}