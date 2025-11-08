using Microsoft.EntityFrameworkCore;
using ShpaginApp.Models.Entities;

namespace ShpaginApp.Data.Repositories
{
  public class TagRepository(ShpaginAppContext context)
  {
    private readonly ShpaginAppContext _context = context;

    private IQueryable<Tag> Get()
    {
      return _context.Tags
        .AsNoTracking()
        .Include(t => t.BookTags).ThenInclude(bt => bt.Book)
        .OrderBy(t => t.Value);
    }

    public async Task<Tag?> GetByIdAsync(Guid id)
    {
      return await Get().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
      return await Get().ToListAsync();
    }

    public async Task<Tag> CreateAsync(Tag tag)
    {
      var entry = await _context.Tags.AddAsync(tag);
      await _context.SaveChangesAsync();
      return entry.Entity;
    }

    public async Task<Tag> UpdateAsync(Tag tag)
    {
      var entry = _context.Tags.Update(tag);
      await _context.SaveChangesAsync();
      return entry.Entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
      var entity = await GetByIdAsync(id);
      if (entity == null) return false;

      _context.Tags.Remove(entity);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistAsync(Guid id)
    {
      return await _context.Tags.AnyAsync(t => t.Id == id);
    }

    public async Task<bool> ExistAsync(string value)
    {
      return await _context.Tags.AnyAsync(t => t.Value == value);
    }
  }
}
