namespace ShpaginApp.Models.Entities
{
  public class Author
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }

    public List<BookAuthor> BookAuthors { get; } = [];
  }

  public class BookAuthor
  {
    public required Guid BookId { get; set; }
    public required Guid AuthorId { get; set; }

    public Book Book { get; set; } = null!;
    public Author Author { get; set; } = null!;
  }
}
