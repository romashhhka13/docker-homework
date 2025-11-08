namespace ShpaginApp.Models.Entities
{
  public class Book
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required int Year { get; set; }
    public required RussianAgeRatingEnum AgeRating { get; set; }
    public string? Publisher { get; set; }
    public string? Annotation { get; set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }

    public List<Author> Authors { get; } = [];
    public List<BookAuthor> BookAuthors { get; } = [];

    public List<Tag> Tags { get; } = [];
    public List<BookTag> BookTags { get; } = [];

    public List<UserBook> UserBooks { get; } = [];
  }
}
