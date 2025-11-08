using Riok.Mapperly.Abstractions;
using ShpaginApp.Models.Entities;
using ShpaginApp.Models.DTOs;

namespace ShpaginApp.Models.Mapping
{
  [Mapper]
  public static partial class TagMapper
  {
    [MapperIgnoreSource(nameof(Tag.BookTags))]
    public static partial TagResponse Map(Tag tag);

    [MapperIgnoreTarget(nameof(Tag.Id))]
    public static partial Tag Map(CreateTagRequest request);

    [MapperIgnoreTarget(nameof(Tag.Id))]
    public static partial void ApplyUpdate([MappingTarget] this Tag tag, UpdateTagRequest request);
  }
}
