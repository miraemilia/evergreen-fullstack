using Evergreen.Core.src.Enum;

namespace Evergreen.Service.src.DTO;

public class UserUpdateDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }

    public UserRole? Role { get; set; }
}