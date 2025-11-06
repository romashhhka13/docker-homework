using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace ShpaginApp.Extentions
{
  internal static class SwaggerGenWithAuthExtentions
  {
    public static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
    {
      services.AddSwaggerGen(o =>
        {
          var securitySchema = new OpenApiSecurityScheme
          {
            Name = "Authorization",
            Description = "Enter yout JWT token",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme.ToLower(),
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
              Type = ReferenceType.SecurityScheme,
              Id = JwtBearerDefaults.AuthenticationScheme
            }
          };
          o.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
          o.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
              {
                new OpenApiSecurityScheme
                {
                  Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme,
                  }
                },
                []
              }
            }
          );
        }
      );


      return services;
    }
  }
}
