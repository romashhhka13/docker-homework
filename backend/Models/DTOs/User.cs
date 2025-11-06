using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShpaginApp.Models.Validators;

namespace ShpaginApp.Models.DTOs
{
  // ----- User ------ //
  public record UserEntity(
    Guid Id,
    bool IsAdmin,
    string UserName,
    string PasswordHash,
    string Email,
    string? DisplayName,
    DateOnly? BirthDate,
    DateTime CreatedAt,
    DateTime UpdatedAt
  );

  public record UserItemResponse(
    Guid Id,
    bool IsAdmin,
    string UserName,
    string Email
  );

  public record UserResponse(
    Guid Id,
    bool IsAdmin,
    string UserName,
    string Email,
    string? DisplayName,
    DateOnly? BirthDate,
    DateTime CreatedAt,
    DateTime UpdatedAt
  );

  public record CreateUserRequest : RegisterRequest
  {
    [Required]
    public bool? IsAdmin { get; init; }
  };

  public record CreateUserCommand(
    bool IsAdmin,
    string UserName,
    string PasswordHash,
    string Email,
    string? DisplayName,
    DateOnly? BirthDate
  );

  public record UpdateUserPutRequest
  {
    [Required]
    public bool? IsAdmin { get; init; }

    [Required, RegularExpression(@"^[a-z][a-z0-9_]{4,31}$",
      ErrorMessage = "You can use a-z, 0-9 and _. Must start with a-z, minimum 5 characters, maximum 32")]
    public string? UserName { get; init; } = null!;

    [Required, RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()_\-+=]{8,32}$",
      ErrorMessage = "You can use a-z, A-Z, 0-9 and !@#$%^&*()_\\-+=. Minimum 8 characters, maximum 32")]
    public string? Password { get; init; } = null!;

    [Required, EmailAddress]
    public string? Email { get; init; } = null!;

    [JsonRequired, MaxLength(32)]
    public string? DisplayName { get; init; }

    [JsonRequired, Date]
    public string? BirthDate { get; init; }
  };

  public record UpdateUserPatchRequest
  {
    public bool? IsAdmin { get; init; }

    [RegularExpression(@"^[a-z][a-z0-9_]{4,31}$",
      ErrorMessage = "You can use a-z, 0-9 and _. Must start with a-z, minimum 5 characters, maximum 32")]
    public string? UserName { get; init; }

    [RegularExpression(@"^[a-zA-Z0-9!@#$%^&*()_\-+=]{8,32}$",
      ErrorMessage = "You can use a-z, A-Z, 0-9 and !@#$%^&*()_\\-+=. Minimum 8 characters, maximum 32")]
    public string? Password { get; init; }

    [EmailAddress]
    public string? Email { get; init; }

    [MaxLength(32)]
    public string? DisplayName { get; init; }

    [Date]
    public string? BirthDate { get; init; }
  };
  // ----- User ------ //


  // ----- UserBook ------ //
  public record UserBookResponse(
    Guid Id,
    string Name,
    bool? Favorite,
    int? Rating,
    BookStatusEnum? Status,
    DateTime AddedAt,
    DateTime UpdatedAt
  );

  public record AddUserBookRequest
  {
    [Required]
    public Guid BookId { get; init; }

    public bool? Favorite { get; init; }

    [Range(1, 5)]
    public int? Rating { get; init; }

    [Enum(typeof(BookStatusEnum))]
    public BookStatusEnum? Status { get; init; }
  };

  public record UpdateUserBookPutRequest
  {
    [JsonRequired]
    public bool? Favorite { get; init; } = null!;

    [JsonRequired, Range(1, 5)]
    public int? Rating { get; init; } = null!;

    [JsonRequired, Enum(typeof(BookStatusEnum))]
    public BookStatusEnum? Status { get; init; } = null!;
  };

  public record UpdateUserBookPatchRequest
  {
    public bool? Favorite { get; init; }

    [Range(1, 5)]
    public int? Rating { get; init; }

    [Enum(typeof(BookStatusEnum))]
    public BookStatusEnum? Status { get; init; }
  };
  // ----- UserBook ------ //
}
