using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ShpaginApp.Auth
{
  internal static class AuthExtentions
  {
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {

      var authOptions = configuration.GetSection("AuthSettings").Get<AuthOptions>()
        ?? throw new InvalidOperationException("AuthSettings section is missing in configuration.");

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = authOptions.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecretKey)),
          };
        });

      return services;
    }
  }
}
