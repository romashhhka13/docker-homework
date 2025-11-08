using ShpaginApp.Auth;
using ShpaginApp.Data.Repositories;
using ShpaginApp.Exceptions;
using ShpaginApp.Helpers;
using ShpaginApp.Models.Mapping;
using ShpaginApp.Models.DTOs;

namespace ShpaginApp.Data.Services
{
  public class AuthService(UserService userService, UserRepository userRepo, JwtService jwtService)
  {
    private readonly UserRepository _userRepo = userRepo;
    private readonly UserService _userService = userService;
    private readonly JwtService _jwtService = jwtService;

    public async Task<UserResponse> Register(CreateUserCommand command)
    {
      return await _userService.Create(command);
    }

    public async Task<string> Login(LoginCommand request)
    {
      static AppException Unauthorized() => new(StatusCodes.Status401Unauthorized, "Invalid credentials");

      var user = await _userRepo.GetByUserNameAsync(request.UserName)
        ?? throw Unauthorized();

      if (!PasswordHelper.VerifyPassword(request.PasswordHash, user.PasswordHash))
        throw Unauthorized();

      return _jwtService.GenerateToken(UserMapper.MapToEntity(user));
    }
  }
}
