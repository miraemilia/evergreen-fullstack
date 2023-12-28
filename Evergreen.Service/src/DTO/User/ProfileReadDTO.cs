using Evergreen.Core.src.Enum;

namespace Evergreen.Service.src.DTO;

public class ProfileReadDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public UserRole Role { get; set; }
    public IEnumerable<OrderReadDTO> Orders { get; set; }
}