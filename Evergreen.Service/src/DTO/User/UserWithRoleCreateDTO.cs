using Evergreen.Core.src.Enum;

namespace Evergreen.Service.src.DTO;

public class UserWithRoleCreateDTO
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Avatar { get; set; }
    public UserRole Role { get; set; }
}