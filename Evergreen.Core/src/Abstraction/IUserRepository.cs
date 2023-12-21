using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;

namespace Evergreen.Core.src.Abstraction;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetOneByEmailAsync(string email);
    Task<bool> EmailAvailable(string email);
    Task<int> GetCountAsync(GetAllParams options);
}