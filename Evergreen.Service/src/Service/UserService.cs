using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Enum;
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

    public async Task<UserReadDTO> CreateUserAsync(UserWithRoleCreateDTO userCreateDTO)
    {
        var emailAvailable = await _userRepo.EmailAvailable(userCreateDTO.Email);
        if (!emailAvailable)
        {
            throw CustomException.EmailNotAvailable($"Email {userCreateDTO.Email} is unavailable.");
        }
        PasswordService.HashPassword(userCreateDTO.Password, out string hashedPassword, out byte[] salt);
        var user = _mapper.Map<UserWithRoleCreateDTO, User>(userCreateDTO);
        user.Password = hashedPassword;
        user.Salt = salt;
        var result = await _userRepo.CreateOneAsync(user);
        return _mapper.Map<User, UserReadDTO>(result);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var userToDelete = await _userRepo.GetOneByIdAsync(id);
        if (userToDelete != null)
        {
            return await _userRepo.DeleteOneAsync(userToDelete);            
        }
        throw CustomException.NotFoundException("User not found");
    }

    public async Task<bool> EmailAvailableAsync(string email)
    {
        return await _userRepo.EmailAvailable(email);
    }

    public async Task<UserPageableReadDTO> GetAllUsersAsync(GetAllParams options)
    {
        var result = await _userRepo.GetAllAsync(options);
        var total = await _userRepo.GetCountAsync(options);
        var foundUsers = _mapper.Map<IEnumerable<User>, IEnumerable<UserReadDTO>>(result);
        int pages = (total + options.Limit -1)/options.Limit;
        var response = new UserPageableReadDTO(){Items = foundUsers, TotalItems = total, TotalPages = pages};
        return response;
    }

    public async Task<UserReadDTO> GetUserByIdAsync(Guid id)
    {
        var result = await _userRepo.GetOneByIdAsync(id);
        if (result != null)
        {
            return _mapper.Map<User, UserReadDTO>(result);
        }
        else
        {
            throw CustomException.NotFoundException("User not found");
        }
    }

    public async Task<UserReadDTO> UpdateUserRoleAsync(Guid id, UserRole newRole)
    {
        var userToUpdate = await _userRepo.GetOneByIdAsync(id);
        if (userToUpdate != null)
        {
            userToUpdate.Role = newRole;
            var updated = await _userRepo.UpdateOneAsync(userToUpdate);
            return _mapper.Map<User, UserReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("User not found");
        }
    }
}