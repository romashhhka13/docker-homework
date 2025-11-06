using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShpaginApp.Models.DTOs;

namespace ShpaginApp.Auth
{
  public class JwtService(IOptions<AuthOptions> options)
  {
    public string GenerateToken(UserEntity user)
    {
      var now = DateTime.UtcNow;

      var claims = new List<Claim>
      {
        new("id", user.Id.ToString()),
        new("name", user.UserName),
        new("role", user.IsAdmin ? AuthRoles.Admin : AuthRoles.User),
      };
      var identity = new ClaimsIdentity(claims, "Token", "name", "role");
      var principal = new ClaimsPrincipal(identity);

      var jwtToken = new JwtSecurityToken(
        issuer: options.Value.Issuer,
        audience: options.Value.Audience,
        notBefore: now,
        expires: now.Add(options.Value.Lifetime),
        claims: principal.Claims,
        signingCredentials: new SigningCredentials(
          new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
          SecurityAlgorithms.HmacSha256)
      );

      return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
  }
}
