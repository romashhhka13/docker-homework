namespace ShpaginApp.Models.Entities
{
  public class User
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public required bool IsAdmin { get; set; }
    public required string UserName { get; set; }
    public required string PasswordHash { get; set; }
    public required string Email { get; set; }
    public string? DisplayName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }

    public List<UserBook> UserBooks { get; } = [];
  }

  public class UserBook
  {
    public required Guid UserId { get; set; }
    public required Guid BookId { get; set; }
    public bool? Favorite { get; set; }
    public int? Rating { get; set; }
    public BookStatusEnum? Status { get; set; }
    public DateTime AddedAt { get; }
    public DateTime UpdatedAt { get; }

    public User User { get; set; } = null!;
    public Book Book { get; set; } = null!;
  }
}
