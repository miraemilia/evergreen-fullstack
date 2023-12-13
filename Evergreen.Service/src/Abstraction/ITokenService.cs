using Evergreen.Core.src.Entity;

namespace Evergreen.Service.src.Abstraction;

public interface ITokenService
{
    string GenerateToken(User user);
}