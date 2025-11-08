using Riok.Mapperly.Abstractions;
using ShpaginApp.Models.Entities;
using ShpaginApp.Models.DTOs;

namespace ShpaginApp.Models.Mapping
{
  [Mapper]
  [UseStaticMapper(typeof(AuthorMapper))]
  [UseStaticMapper(typeof(TagMapper))]
  public static partial class BookMapper
  {
    [MapValue(nameof(BookResponse.Statistics), null)]
    [MapperIgnoreSource(nameof(Book.BookAuthors))]
    [MapperIgnoreSource(nameof(Book.BookTags))]
    [MapperIgnoreSource(nameof(Book.UserBooks))]
    public static partial BookResponse Map(Book book);

    [MapperIgnoreSource(nameof(Book.BookAuthors))]
    [MapperIgnoreSource(nameof(Book.BookTags))]
    [MapperIgnoreSource(nameof(Book.UserBooks))]
    public static partial BookResponse Map(Book book, BookStatistics statistics);

    [MapperIgnoreSource(nameof(Book.Year))]
    [MapperIgnoreSource(nameof(Book.AgeRating))]
    [MapperIgnoreSource(nameof(Book.Annotation))]
    [MapperIgnoreSource(nameof(Book.Publisher))]
    [MapperIgnoreSource(nameof(Book.CreatedAt))]
    [MapperIgnoreSource(nameof(Book.UpdatedAt))]
    [MapperIgnoreSource(nameof(Book.BookAuthors))]
    [MapperIgnoreSource(nameof(Book.BookTags))]
    [MapperIgnoreSource(nameof(Book.Tags))]
    [MapperIgnoreSource(nameof(Book.UserBooks))]
    public static partial BookItemResponse MapItem(Book book);

    [MapperIgnoreTarget(nameof(Book.Id))]
    [MapperIgnoreSource(nameof(CreateBookRequest.AuthorIds))]
    [MapperIgnoreSource(nameof(CreateBookRequest.TagIds))]
    public static partial Book Map(CreateBookRequest request);

    [MapperIgnoreTarget(nameof(Book.Id))]
    [MapperIgnoreSource(nameof(UpdateBookPatchRequest.AuthorIds))]
    [MapperIgnoreSource(nameof(UpdateBookPatchRequest.TagIds))]
    public static partial void ApplyUpdate([MappingTarget] this Book book, UpdateBookPutRequest request);

    [MapperIgnoreTarget(nameof(Book.Id))]
    [MapperIgnoreSource(nameof(UpdateBookPatchRequest.AuthorIds))]
    [MapperIgnoreSource(nameof(UpdateBookPatchRequest.TagIds))]
    public static partial void ApplyUpdate([MappingTarget] this Book book, UpdateBookPatchRequest request);
  }
}
