using Microsoft.EntityFrameworkCore;
using ShpaginApp.Models;
using ShpaginApp.Models.Entities;
using ShpaginApp.Models.DTOs;
using ShpaginApp.Exceptions;

namespace ShpaginApp.Data.Repositories
{
  public class BookRepository(ShpaginAppContext context)
  {
    private readonly ShpaginAppContext _context = context;

    public IQueryable<Book> Get()
    {
      return _context.Books
        .AsNoTracking()
        .Include(b => b.Authors)
        .Include(b => b.Tags)
        .OrderBy(b => b.Name);
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
      return await Get().FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
      return await Get().ToListAsync();
    }

    public async Task<BookStatistics> GetStatisticsByIdAsync(Guid id)
    {
      var query = _context.UsersBooks.AsNoTracking().Where(ub => ub.BookId == id);

      var ratings = query.Where(ub => ub.Rating.HasValue).Select(ub => ub.Rating!.Value);
      double averageRating = await ratings.AnyAsync() ? await ratings.AverageAsync() : 0;

      return new BookStatistics(
        AverageRating: averageRating,
        InFavorites: await query.CountAsync(ub => ub.Favorite == true),
        TotalReaders: await query.CountAsync(),
        WantToRead: await query.CountAsync(ub => ub.Status == BookStatusEnum.WANT_TO_READ),
        ReadingNow: await query.CountAsync(ub => ub.Status == BookStatusEnum.READING),
        Completed: await query.CountAsync(ub => ub.Status == BookStatusEnum.COMPLETED)
      );
    }

    public IQueryable<Book> SearchBooksQuery(BookSearchRequest request)
    {
      var query = _context.Books
        .AsNoTracking()
        .Include(b => b.Authors)
        .Include(b => b.Tags)
        .Include(b => b.UserBooks)
        .OrderBy(b => b.Name)
        .AsQueryable();

      if (!string.IsNullOrWhiteSpace(request.Query))
        query = query.Where(b => b.Name.Contains(request.Query));

      if (request.TagIds != null && request.TagIds.Any())
        query = query.Where(b => b.Tags.Any(t => request.TagIds.Contains(t.Id)));

      if (request.AuthorIds != null && request.AuthorIds.Any())
        query = query.Where(b => b.Authors.Any(a => request.AuthorIds.Contains(a.Id)));

      if (request.Count != null && request.Count.HasValue)
        query = query.Take(request.Count.Value);

      return query.Distinct().OrderBy(b => b.Name);
    }

    public async Task<Book> CreateAsync(Book book, IEnumerable<Guid> authorIds, IEnumerable<Guid>? tagIds)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        var entry = await _context.Books.AddAsync(book);

        var existingAuthorIds = await _context.Authors
            .Where(a => authorIds.Contains(a.Id))
            .Select(a => a.Id)
            .ToListAsync();

        var missingAuthorIds = authorIds.Except(existingAuthorIds).ToList();
        if (missingAuthorIds.Count != 0)
        {
          throw new AppException(StatusCodes.Status400BadRequest,
            $"Authors with ids << {string.Join(", ", missingAuthorIds)} >> not found");
        }

        var bookAuthors = existingAuthorIds.Select(authorId => new BookAuthor
        {
          BookId = entry.Entity.Id,
          AuthorId = authorId
        }).ToList();

        await _context.BooksAuthors.AddRangeAsync(bookAuthors);

        if (tagIds != null)
        {
          var existingTagIds = await _context.Tags
              .Where(t => tagIds.Contains(t.Id))
              .Select(t => t.Id)
              .ToListAsync();

          var missingTagIds = tagIds.Except(existingTagIds).ToList();
          if (missingTagIds.Count != 0)
          {
            throw new AppException(StatusCodes.Status400BadRequest,
              $"Tags with ids << {string.Join(", ", missingTagIds)} >> not found");
          }

          var bookTags = existingTagIds.Select(tagId => new BookTag
          {
            BookId = entry.Entity.Id,
            TagId = tagId
          }).ToList();

          await _context.BooksTags.AddRangeAsync(bookTags);
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return await _context.Books
        .Include(b => b.Authors)
        .Include(b => b.Tags)
        .FirstAsync(b => b.Id == entry.Entity.Id);
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }

    public async Task<Book> UpdateAsync(Book book, IEnumerable<Guid>? authorIds, IEnumerable<Guid>? tagIds)
    {
      using var transaction = await _context.Database.BeginTransactionAsync();

      try
      {
        var existing = await _context.Books
          .Include(b => b.Authors)
          .Include(b => b.Tags)
          .FirstAsync(b => b.Id == book.Id);

        _context.Entry(existing).CurrentValues.SetValues(book);

        if (authorIds != null)
        {
          var existingAuthors = await _context.Authors
            .Where(a => authorIds.Contains(a.Id))
            .ToListAsync();

          var existingAuthorIds = existingAuthors.Select(a => a.Id).ToHashSet();
          var missingAuthorIds = authorIds.Except(existingAuthorIds).ToList();

          if (missingAuthorIds.Count != 0)
          {
            throw new AppException(StatusCodes.Status400BadRequest,
              $"Authors with ids << {string.Join(", ", missingAuthorIds)} >> not found");
          }

          var currentAuthorIds = existing.Authors.Select(a => a.Id).ToHashSet();

          foreach (var author in existing.Authors.Where(a => !existingAuthorIds.Contains(a.Id)).ToList())
            existing.Authors.Remove(author);

          foreach (var author in existingAuthors.Where(a => !currentAuthorIds.Contains(a.Id)))
            existing.Authors.Add(author);
        }

        if (tagIds != null)
        {
          var existingTags = await _context.Tags
            .Where(t => tagIds.Contains(t.Id))
            .ToListAsync();

          var existingTagIds = existingTags.Select(a => a.Id).ToHashSet();
          var missingTagIds = tagIds.Except(existingTagIds).ToList();
          if (missingTagIds.Count != 0)
          {
            throw new AppException(StatusCodes.Status400BadRequest,
                $"Tags with ids << {string.Join(", ", missingTagIds)} >> not found");
          }

          var currentTagIds = existing.Tags.Select(t => t.Id).ToHashSet();

          foreach (var tag in existing.Tags.Where(t => !existingTagIds.Contains(t.Id)).ToList())
            existing.Tags.Remove(tag);

          foreach (var tag in existingTags.Where(t => !currentTagIds.Contains(t.Id)))
            existing.Tags.Add(tag);
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return existing;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }


    public async Task<bool> DeleteAsync(Guid id)
    {
      var entity = await GetByIdAsync(id);
      if (entity == null) return false;

      _context.Books.Remove(entity);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistAsync(Guid id)
    {
      return await _context.Books.AnyAsync(b => b.Id == id);
    }
  }
}
