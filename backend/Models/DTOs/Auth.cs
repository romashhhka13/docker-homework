using System.ComponentModel.DataAnnotations;
using ShpaginApp.Models.Validators;

namespace ShpaginApp.Models.DTOs
{
  public record RegisterRequest
  {
    [Required]
    [RegularExpression(@"^[a-z][a-z0-9_]{4,31}$",
      ErrorMessage = "You can use a-z, 0-9 and _. Must start with a-z, minimum 5 characters, maximum 32")]
    public string? UserName { get; init; } = null!;

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()_\-+=]{8,32}$",
      ErrorMessage = "You can use a-z, A-Z, 0-9 and !@#$%^&*()_\\-+=. Minimum 8 characters, maximum 32")]
    public string? Password { get; init; } = null!;

    [Required, EmailAddress]
    public string? Email { get; init; } = null!;

    [MaxLength(32)]
    public string? DisplayName { get; init; }

    [Date]
    public string? BirthDate { get; init; }
  };

  public record LoginRequest
  {
    [Required]
    public string UserName { get; init; } = null!;

    [Required]
    public string? Password { get; init; } = null!;

  }

  public record LoginCommand(string UserName, string PasswordHash);

  public record LoginResponse(string AccessToken);
}
