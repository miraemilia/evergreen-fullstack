using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IAuthService
{
        Task<string> Login(LoginParams loginParams);
        Task<ProfileReadDTO> GetProfileAsync(Guid id);
        Task<ProfileReadDTO> CreateProfileAsync(UserCreateDTO newUser);
        Task<ProfileReadDTO> UpdateProfileAsync(Guid id, UserUpdateDTO update);
        Task<ProfileReadDTO> ChangePasswordAsync(Guid id, string password);
        Task<bool> DeleteProfileAsync(Guid id);
}