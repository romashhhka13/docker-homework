using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShpaginApp.Models.DTOs
{
  public record AuthorItemResponse(
    Guid Id,
    string Name
  );

  public record AuthorResponse(
    Guid Id,
    string Name,
    string? Description
  );

  public record CreateAuthorRequest
  {
    [Required, MaxLength(255)]
    public string Name { get; init; } = null!;

    [MaxLength(1023)]
    public string? Description { get; init; }
  };

  public record UpdateAuthorPutRequest
  {
    [Required, MaxLength(255)]
    public string? Name { get; init; } = null!;

    [JsonRequired, MaxLength(1023)]
    public string? Description { get; init; }
  };

  public record UpdateAuthorPatchRequest
  {
    [MaxLength(255)]
    public string? Name { get; init; }

    [MaxLength(1023)]
    public string? Description { get; init; }
  };
}
