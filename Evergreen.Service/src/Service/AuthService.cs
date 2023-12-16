using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Entity;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
using Evergreen.Service.src.DTO;
using Evergreen.Service.src.Shared;

namespace Evergreen.Service.src.Service;

public class AuthService : IAuthService
{
    private IUserRepository _userRepo;
    private IMapper _mapper;
    private ITokenService _tokenService;

    public AuthService(IUserRepository userRepo, IMapper mapper, ITokenService tokenService)
    {
        _userRepo = userRepo;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<string> Login(LoginParams loginParams)
    {
        var foundUser = await _userRepo.GetOneByEmailAsync(loginParams.Email);
        if (foundUser == null)
        {
            throw CustomException.WrongCredentialsException("Wrong email");
        }
        var isPasswordMatch = PasswordService.VerifyPassword(loginParams.Password, foundUser.Password, foundUser.Salt);
        if (isPasswordMatch)
        {
            try
            {
                var token = _tokenService.GenerateToken(foundUser);
                return token;
            }
            catch
            {
                throw CustomException.TokenNotCreated();
            }

        }
        throw CustomException.WrongCredentialsException("Wrong password");
    }

    public async Task<UserReadDTO> UpdateProfileAsync(Guid id, UserUpdateDTO update)
    {
        var userToUpdate = await _userRepo.GetOneByIdAsync(id);
        if (userToUpdate != null)
        {
            if (update.Email != null)
            {
                var emailAvailable = await _userRepo.EmailAvailable(update.Email);
                if (!emailAvailable)
                {
                    throw CustomException.EmailNotAvailable($"Email {update.Email} is unavailable.");
                }
            }
            //var updatedUser = _mapper.Map(update, userToUpdate);
            var updatedUser = update.Merge(userToUpdate);
            var updated = await _userRepo.UpdateOneAsync(updatedUser);
            return _mapper.Map<User, UserReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("User not found");
        }
        
    }

    public async Task<UserReadDTO> GetProfileAsync(Guid id)
    {
        var foundUser = await _userRepo.GetOneByIdAsync(id);
        if (foundUser != null)
        {
            return _mapper.Map<User, UserReadDTO>(foundUser);            
        }
        throw CustomException.NotFoundException("User not found");
    }

    public async Task<UserReadDTO> ChangePasswordAsync(Guid id, string newPassword)
    {
        var userToUpdate = await _userRepo.GetOneByIdAsync(id);
        if (userToUpdate != null)
        {
            PasswordService.HashPassword(newPassword, out string hashedPassword, out byte[] salt);
            userToUpdate.Password = hashedPassword;
            userToUpdate.Salt = salt;
            var updated = await _userRepo.UpdateOneAsync(userToUpdate);
            return _mapper.Map<User, UserReadDTO>(updated);
        }
        else
        {
            throw CustomException.NotFoundException("User not found");
        }
    }

    public async Task<bool> DeleteProfileAsync(Guid id)
    {
        var userToDelete = await _userRepo.GetOneByIdAsync(id);
        if (userToDelete != null)
        {
            return await _userRepo.DeleteOneAsync(userToDelete);            
        }
        throw CustomException.NotFoundException("User not found");
    }

    public async Task<UserReadDTO> CreateProfileAsync(UserCreateDTO newUser)
    {
        var emailAvailable = await _userRepo.EmailAvailable(newUser.Email);
        if (!emailAvailable)
        {
            throw CustomException.EmailNotAvailable($"Email {newUser.Email} is unavailable.");
        }
        PasswordService.HashPassword(newUser.Password, out string hashedPassword, out byte[] salt);
        var user = _mapper.Map<UserCreateDTO, User>(newUser);
        user.Password = hashedPassword;
        user.Salt = salt;
        var result = await _userRepo.CreateOneAsync(user);
        return _mapper.Map<User, UserReadDTO>(result);
    }
}