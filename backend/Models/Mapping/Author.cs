using Riok.Mapperly.Abstractions;
using ShpaginApp.Models.Entities;
using ShpaginApp.Models.DTOs;

namespace ShpaginApp.Models.Mapping
{
  [Mapper]
  public static partial class AuthorMapper
  {
    [MapperIgnoreSource(nameof(Author.Description))]
    [MapperIgnoreSource(nameof(Author.BookAuthors))]
    public static partial AuthorItemResponse MapItem(Author author);

    [MapperIgnoreSource(nameof(Author.BookAuthors))]
    public static partial AuthorResponse Map(Author author);

    [MapperIgnoreTarget(nameof(Author.Id))]
    public static partial Author Map(CreateAuthorRequest request);

    [MapperIgnoreTarget(nameof(Author.Id))]
    public static partial void ApplyUpdate([MappingTarget] this Author author, UpdateAuthorPutRequest request);

    [MapperIgnoreTarget(nameof(Author.Id))]
    public static partial void ApplyUpdate([MappingTarget] this Author author, UpdateAuthorPatchRequest request);
  }
}
