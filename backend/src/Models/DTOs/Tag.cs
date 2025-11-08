using System.ComponentModel.DataAnnotations;

namespace ShpaginApp.Models.DTOs
{
  public record TagResponse(Guid Id, string Value);

  public record CreateTagRequest([Required, MaxLength(64)] string Value);

  public record UpdateTagRequest([Required, MaxLength(64)] string? Value);
}
