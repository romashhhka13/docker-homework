using Riok.Mapperly.Abstractions;
using ShpaginApp.Helpers;
using ShpaginApp.Models.DTOs;

namespace ShpaginApp.Models.Mapping
{
  [Mapper]
  public static partial class AuthMapper
  {
    [MapValue(nameof(CreateUserCommand.IsAdmin), false)]
    [MapProperty(nameof(RegisterRequest.Password), nameof(CreateUserCommand.PasswordHash), Use = nameof(GetPasswordHash))]
    public static partial CreateUserCommand Map(RegisterRequest request);

    [MapProperty(nameof(LoginRequest.Password), nameof(LoginCommand.PasswordHash), Use = nameof(GetPasswordHash))]
    public static partial LoginCommand Map(LoginRequest request);

    [UserMapping(Default = false)]
    private static string GetPasswordHash(string password) => PasswordHelper.HashPassword(password);
  }
}
