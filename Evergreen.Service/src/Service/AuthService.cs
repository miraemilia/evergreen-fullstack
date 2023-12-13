using AutoMapper;
using Evergreen.Core.src.Abstraction;
using Evergreen.Core.src.Parameter;
using Evergreen.Service.src.Abstraction;
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
/*         var foundUser = await _userRepo.GetOneByEmailAsync(loginParams.Email);
        if (foundUser == null)
        {
            throw CustomException.WrongCredentialsException("Wrong email");
        }
        var isPasswordMatch = PasswordService.VerifyPassword(loginParams.Password, foundUser.Password, foundUser.Salt);
        if (isPasswordMatch)
        {
            var token = _tokenService.GenerateToken(foundUser);
            return token;
        }
        throw CustomException.WrongCredentialsException("Wrong password"); */
        return "token";
    }
}