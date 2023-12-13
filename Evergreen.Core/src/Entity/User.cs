using Evergreen.Core.src.Enum;

namespace Evergreen.Core.src.Entity;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    //public byte[] Salt { get; set; }
    public string Avatar { get; set; }
    public UserRole Role { get; set; }  = UserRole.Customer;
    public IEnumerable<Order> Orders { get; set; }
}