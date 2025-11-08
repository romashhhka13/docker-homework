using Microsoft.EntityFrameworkCore;
using ShpaginApp.Models.Entities;

namespace ShpaginApp.Data.Repositories
{
  public class AuthorRepository(ShpaginAppContext context)
  {
    private readonly ShpaginAppContext _context = context;

    public IQueryable<Author> Get()
    {
      return _context.Authors
        .AsNoTracking()
        .Include(a => a.BookAuthors).ThenInclude(ab => ab.Book)
        .OrderBy(a => a.Name);
    }

    public async Task<Author?> GetByIdAsync(Guid id)
    {
      return await Get().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
      return await Get().ToListAsync();
    }

    public async Task<Author> CreateAsync(Author author)
    {
      var entry = await _context.Authors.AddAsync(author);
      await _context.SaveChangesAsync();
      return entry.Entity;
    }

    public async Task<Author> UpdateAsync(Author author)
    {
      var entry = _context.Authors.Update(author);
      await _context.SaveChangesAsync();
      return entry.Entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
      var entity = await GetByIdAsync(id);
      if (entity == null) return false;

      _context.Authors.Remove(entity);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistAsync(Guid id)
    {
      return await _context.Authors.AnyAsync(a => a.Id == id);
    }
  }
}
