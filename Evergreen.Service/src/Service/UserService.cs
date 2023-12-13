using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Shared;

namespace Evergreen.Service.src.Service;

public class UserService : IUserService
{
    private IUserRepository _userRepo;
    private IMapper _mapper;

    public UserService(IUserRepository userRepo, IMapper mapper)
    {
        _userRepo = userRepo;
        _mapper = mapper;
    }

    //add exception
    public async Task<UserReadDTO> CreateUserAsync(UserCreateDTO userCreateDTO)
    {
        //Console.WriteLine("creating hash");
        //PasswordService.HashPassword(userCreateDTO.Password, out string hashedPassword, out byte[] salt);
        //Console.WriteLine("hash created");
        var user = _mapper.Map<UserCreateDTO, User>(userCreateDTO);
        //user.Password = hashedPassword;
        //user.Salt = salt;
        var result = await _userRepo.CreateOneAsync(user);
        return _mapper.Map<User, UserReadDTO>(result);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        return await _userRepo.DeleteOneAsync(id);
    }

    public async Task<bool> EmailAvailableAsync(string email)
    {
        var found = await _userRepo.GetOneByEmailAsync(email);
        if (found is null)
        {
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<UserReadDTO>> GetAllUsersAsync(GetAllParams options)
    {
        var result = await _userRepo.GetAllAsync(options);
        return _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDTO>>(result);
    }

    public async Task<UserReadDTO> GetUserByIdAsync(Guid id)
    {
        var result = await _userRepo.GetOneByIdAsync(id);
        if (result != null)
        {
            var dto = _mapper.Map<User, UserReadDTO>(result);
            return dto;
        }
        else
        {
            throw CustomException.NotFoundException("User not found");
        }
    }

    // change to use mapper?
    public async Task<UserReadDTO> UpdateUserAsync(Guid id, UserUpdateDTO update)
    {
        var userToUpdate = await _userRepo.GetOneByIdAsync(id);
        if (userToUpdate != null)
        {
            if (update.Name is not null)
            {
                userToUpdate.Name = update.Name;
            }
            if (update.Email is not null && await EmailAvailableAsync(update.Email))
            {
                userToUpdate.Email = update.Email;
            }
            if (update.Avatar is not null)
            {
                userToUpdate.Avatar = update.Avatar;
            }
            if (update.Role is not null)
            {
                userToUpdate.Role = (Core.src.Enum.UserRole)update.Role;
            }
            var updated = await _userRepo.UpdateOneAsync(userToUpdate);
            return _mapper.Map<User, UserReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("User not found");
        }
        

    }
}