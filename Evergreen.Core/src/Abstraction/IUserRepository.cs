using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
using Evergreen.Core.src.Parameter;

namespace Evergreen.Core.src.Abstraction;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers(GetAllParams options);
    User GetUserById(Guid id);
    User GetUserByCredentials(string email, string password);
    User AddUser(User user);
    bool DeleteUser(Guid id);
    User UpdateUser(Guid id, User user);
    string GenerateToken(User user);
    bool EmailAvailable(string email);
}