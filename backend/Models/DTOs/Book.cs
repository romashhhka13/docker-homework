using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShpaginApp.Models.Validators;

namespace ShpaginApp.Models.DTOs
{
  public record BookItemResponse(
    Guid Id,
    string Name,
    IEnumerable<AuthorItemResponse> Authors
  );

  public record BookResponse
  {
    public Guid Id { get; init; }

    public required string Name { get; init; }

    // [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Year { get; init; }

    public RussianAgeRatingEnum? AgeRating { get; init; }

    public string? Publisher { get; init; }

    public string? Annotation { get; init; }

    public required IEnumerable<AuthorItemResponse> Authors { get; init; }

    public IEnumerable<TagResponse>? Tags { get; init; }

    public DateTime? CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }

    public BookStatistics? Statistics { get; init; }

  };

  public record BookStatistics(
    double AverageRating,
    int InFavorites,
    int TotalReaders,
    int WantToRead,
    int ReadingNow,
    int Completed
  );

  public record CreateBookRequest
  {
    [Required, MaxLength(255)]
    public string? Name { get; init; } = null!;

    [Required, YearRange(minYear: 1000)]
    public int Year { get; init; }

    [Required, Enum(typeof(RussianAgeRatingEnum))]
    public RussianAgeRatingEnum? AgeRating { get; init; } = null!;

    [Required, MinLength(1, ErrorMessage = "At least one author id required")]
    public IEnumerable<Guid> AuthorIds { get; init; } = null!;

    [MinLength(1, ErrorMessage = "At least one tag id required")]
    public IEnumerable<Guid>? TagIds { get; init; }

    [MaxLength(255)]
    public string? Publisher { get; init; }

    [MaxLength(1023)]
    public string? Annotation { get; init; }
  };

  public record UpdateBookPutRequest
  {
    [Required, MaxLength(255)]
    public string Name { get; init; } = null!;

    [Required, YearRange(minYear: 1000)]
    public int? Year { get; init; } = null!;

    [Required, Enum(typeof(RussianAgeRatingEnum))]
    public RussianAgeRatingEnum AgeRating { get; init; }

    [Required, MinLength(1, ErrorMessage = "At least one author id required")]
    public IEnumerable<Guid> AuthorIds { get; init; } = null!;

    [Required]
    public IEnumerable<Guid> TagIds { get; init; } = null!;

    [JsonRequired, MaxLength(255)]
    public string? Publisher { get; init; }

    [JsonRequired, MaxLength(1023)]
    public string? Annotation { get; init; }
  };

  public record UpdateBookPatchRequest
  {
    [MaxLength(255)]
    public string? Name { get; init; }

    [YearRange(minYear: 1000)]
    public int? Year { get; init; }

    [Enum(typeof(RussianAgeRatingEnum))]
    public RussianAgeRatingEnum? AgeRating { get; init; }

    [MinLength(1, ErrorMessage = "At least one author id required")]
    public IEnumerable<Guid>? AuthorIds { get; init; }

    public IEnumerable<Guid>? TagIds { get; init; }

    [MaxLength(255)]
    public string? Publisher { get; init; }

    [MaxLength(1023)]
    public string? Annotation { get; init; }
  };

  public record BookSearchRequest
  {
    [MaxLength(255)]
    public string? Query { get; init; }

    [MinLength(1, ErrorMessage = "At least one author id required")]
    public IEnumerable<Guid>? AuthorIds { get; init; }

    [MinLength(1, ErrorMessage = "At least one tag id required")]
    public IEnumerable<Guid>? TagIds { get; init; }

    [Range(1, 1000)]
    public int? Count { get; init; }
  };
}
