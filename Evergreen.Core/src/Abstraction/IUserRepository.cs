using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;

namespace Evergreen.Core.src.Abstraction;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers(GetAllParams options);
    User GetUser(Guid id);
    User AddUser(User user);
    bool DeleteUser(Guid id);
    User UpdateUser(Guid id, string Name);
    User ChangeUserRole(Guid id, UserRole role);
    string GenerateToken(User user);
    bool EmailAvailable(string email);
}