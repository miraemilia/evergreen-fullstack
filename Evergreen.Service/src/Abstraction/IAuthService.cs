using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.DTO;

namespace Evergreen.Service.src.Abstraction;

public interface IAuthService
{
        Task<string> Login(LoginParams loginParams);
        //Task<UserReadDTO> GetProfile(string token);
}