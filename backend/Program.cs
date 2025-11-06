using System.Text.Json;
using System.Text.Json.Serialization;
using ShpaginApp.Auth;
using ShpaginApp.Data.Repositories;
using ShpaginApp.Data.Services;
using ShpaginApp.Exceptions;
using ShpaginApp.Extentions;
using ShpaginApp.Models;
using ShpaginApp.Models.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);

// ----- Repositories ------ //
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<UserRepository>();

// ----- Services ------ //
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AuthService>();

// ----- Auth ------ //
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthSettings"));
builder.Services.AddAuth(builder.Configuration);

// ----- Controllers ------ //
builder.Services
  .AddControllers(options => options.Filters.Add<ValidateModelAttribute>())
  .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
      options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
  );

// ----- ExceptionHandlers ------ //
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddExceptionHandler<DbExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// ----- Swagger ------ //
builder.Services.AddSwaggerGenWithAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();

  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;
  SeedData.Initialize(services);
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
