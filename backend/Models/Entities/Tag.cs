namespace ShpaginApp.Models.Entities
{
  public class Tag
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Value { get; set; }

    public List<BookTag> BookTags { get; } = [];
  }

  public class BookTag
  {
    public required Guid BookId { get; set; }
    public required Guid TagId { get; set; }

    public Book Book { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
  }
}
