using Evergreen.Core.src.Entity;
namespace Evergreen.Service.src.DTO;

public class UserUpdateDTO
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Avatar { get; set; }

    public User Merge(User user)
    {
        if (Name is not null)
        {
            user.Name = Name;
        }
        if (Email is not null)
        {
            user.Email = Email;
        }
        if (Avatar is not null)
        {
            user.Avatar = Avatar;
        }
        return user;
    }
}