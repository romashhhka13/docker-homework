using Microsoft.EntityFrameworkCore;
using Npgsql;
using ShpaginApp.Data;
using ShpaginApp.Models;

namespace ShpaginApp.Extentions
{
  internal static class DatabaseExtentions
  {
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
      var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("ShpaginAppContext"));

      var enumTranslator = new UpperCaseEnumTranslator();

      // Map Enums
      dataSourceBuilder.MapEnum<RussianAgeRatingEnum>("age_rating", enumTranslator);
      dataSourceBuilder.MapEnum<BookStatusEnum>("book_status", enumTranslator);
      // ...

      var dataSource = dataSourceBuilder.Build();

      services.AddDbContext<ShpaginAppContext>(options =>
          options.UseNpgsql(
              dataSource,
              o =>
              {
                // Map Enums
                o.MapEnum<RussianAgeRatingEnum>("age_rating", "public", enumTranslator);
                o.MapEnum<BookStatusEnum>("book_status", "public", enumTranslator);
                // ...
              }
          )
      );

      return services;
    }
  }
}
