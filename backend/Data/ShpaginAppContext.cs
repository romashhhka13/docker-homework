using Microsoft.EntityFrameworkCore;
using ShpaginApp.Models.Entities;

namespace ShpaginApp.Data
{
  public sealed class ShpaginAppContext(DbContextOptions<ShpaginAppContext> options) : DbContext(options)
  {
    public DbSet<Book> Books { get; set; } = default!;
    public DbSet<Author> Authors { get; set; } = default!;
    public DbSet<Tag> Tags { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;

    public DbSet<BookAuthor> BooksAuthors { get; set; } = default!;
    public DbSet<BookTag> BooksTags { get; set; } = default!;
    public DbSet<UserBook> UsersBooks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Book>(entity =>
      {
        entity.ToTable("books");
        entity.HasKey(b => b.Id).HasName("pk_books");

        entity.Property(b => b.Id).HasColumnName("id");
        entity.Property(b => b.Name).HasColumnName("name").HasColumnType("varchar(255)").IsRequired();
        entity.Property(b => b.Year).HasColumnName("year").HasColumnType("integer").IsRequired();
        entity.Property(b => b.AgeRating).HasColumnName("age_rating").HasColumnType("age_rating").IsRequired();
        entity.Property(b => b.Publisher).HasColumnName("publisher").HasColumnType("varchar(255)").IsRequired(false);
        entity.Property(b => b.Annotation).HasColumnName("annotation").HasColumnType("text").IsRequired(false);
        entity.Property(b => b.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone")
              .HasDefaultValueSql("now()").ValueGeneratedOnAdd();
        entity.Property(b => b.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone")
              .HasDefaultValueSql("now()").ValueGeneratedOnAddOrUpdate();

        entity.HasMany(b => b.Authors).WithMany().UsingEntity<BookAuthor>();
        entity.HasMany(b => b.Tags).WithMany().UsingEntity<BookTag>();
      });

      modelBuilder.Entity<Author>(entity =>
      {
        entity.ToTable("authors");
        entity.HasKey(a => a.Id).HasName("pk_authors");

        entity.Property(a => a.Id).HasColumnName("id");
        entity.Property(a => a.Name).HasColumnName("name").HasColumnType("varchar(255)").IsRequired();
        entity.Property(a => a.Description).HasColumnName("description").HasColumnType("text").IsRequired(false);
      });

      modelBuilder.Entity<Tag>(entity =>
      {
        entity.ToTable("tags");
        entity.HasKey(t => t.Id).HasName("pk_tags");

        entity.Property(t => t.Id).HasColumnName("id");
        entity.Property(t => t.Value).HasColumnName("value").HasColumnType("varchar(64)").IsRequired();
      });

      modelBuilder.Entity<User>(entity =>
      {
        entity.ToTable("users");
        entity.HasKey(u => u.Id).HasName("pk_users");
        entity.HasIndex(u => u.UserName).IsUnique().HasFilter(null).HasDatabaseName("uq_users_username");
        entity.HasIndex(u => u.Email).IsUnique().HasFilter(null).HasDatabaseName("uq_users_email");

        entity.Property(u => u.Id).HasColumnName("id");
        entity.Property(u => u.IsAdmin).HasColumnName("is_admin").HasColumnType("bool").IsRequired();
        entity.Property(u => u.UserName).HasColumnName("username").HasColumnType("varchar(32)").IsRequired();
        entity.Property(u => u.PasswordHash).HasColumnName("password_hash").HasColumnType("varchar(44)").IsRequired();
        entity.Property(u => u.Email).HasColumnName("email").HasColumnType("varchar(255)").IsRequired();
        entity.Property(u => u.DisplayName).HasColumnName("display_name").HasColumnType("varchar(32)").IsRequired(false);
        entity.Property(u => u.BirthDate).HasColumnName("birth_date").HasColumnType("date").IsRequired(false);
        entity.Property(u => u.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp with time zone")
              .HasDefaultValueSql("now()").ValueGeneratedOnAdd();
        entity.Property(u => u.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone")
              .HasDefaultValueSql("now()").ValueGeneratedOnAddOrUpdate();

      });

      modelBuilder.Entity<BookAuthor>(entity =>
      {
        entity.ToTable("books_authors");
        entity.HasKey(ba => new { ba.BookId, ba.AuthorId }).HasName("pk_books_authors");

        entity.Property(ba => ba.BookId).HasColumnName("book_id");
        entity.Property(ba => ba.AuthorId).HasColumnName("author_id");

        entity.HasOne(ba => ba.Book).WithMany(b => b.BookAuthors).HasForeignKey(ba => ba.BookId)
          .HasConstraintName("fk_books_authors_book_id_books");
        entity.HasOne(ba => ba.Author).WithMany(a => a.BookAuthors).HasForeignKey(ba => ba.AuthorId)
          .HasConstraintName("fk_books_authors_author_id_authors");
      });

      modelBuilder.Entity<BookTag>(entity =>
      {
        entity.ToTable("books_tags");
        entity.HasKey(bt => new { bt.BookId, bt.TagId }).HasName("pk_books_tags");

        entity.Property(bt => bt.BookId).HasColumnName("book_id");
        entity.Property(bt => bt.TagId).HasColumnName("tag_id");

        entity.HasOne(bt => bt.Book).WithMany(b => b.BookTags).HasForeignKey(bt => bt.BookId)
          .HasConstraintName("fk_books_tags_book_id_books");
        entity.HasOne(bt => bt.Tag).WithMany(t => t.BookTags).HasForeignKey(bt => bt.TagId)
          .HasConstraintName("fk_books_tags_tag_id_tags");
      });

      modelBuilder.Entity<UserBook>(entity =>
      {
        entity.ToTable("users_books");
        entity.HasKey(ub => new { ub.UserId, ub.BookId }).HasName("pk_users_books");

        entity.Property(ub => ub.UserId).HasColumnName("user_id");
        entity.Property(ub => ub.BookId).HasColumnName("book_id");
        entity.Property(ub => ub.Favorite).HasColumnName("favorite").HasColumnType("bool").IsRequired(false);
        entity.Property(ub => ub.Rating).HasColumnName("rating").HasColumnType("integer").IsRequired(false);
        entity.Property(ub => ub.Status).HasColumnName("status").HasColumnType("book_status").IsRequired(false);
        entity.Property(ub => ub.AddedAt).HasColumnName("added_at").HasColumnType("timestamp with time zone")
          .HasDefaultValueSql("now()").ValueGeneratedOnAdd();
        entity.Property(ub => ub.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp with time zone")
              .HasDefaultValueSql("now()").ValueGeneratedOnAddOrUpdate();

        entity.HasOne(ub => ub.User).WithMany(u => u.UserBooks).HasForeignKey(ub => ub.UserId)
          .HasConstraintName("fk_users_books_user_id_users");
        entity.HasOne(ub => ub.Book).WithMany(b => b.UserBooks).HasForeignKey(ub => ub.BookId)
          .HasConstraintName("fk_users_books_book_id_books");
      });
    }
  }
}
