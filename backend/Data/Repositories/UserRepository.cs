using Microsoft.EntityFrameworkCore;
using ShpaginApp.Models.Entities;
using ShpaginApp.Models.Mapping;
using ShpaginApp.Models.DTOs;

namespace ShpaginApp.Data.Repositories
{
  public class UserRepository(ShpaginAppContext context)
  {
    private readonly ShpaginAppContext _context = context;

    private IQueryable<User> Get()
    {
      return _context.Users
        .AsNoTracking()
        .Include(u => u.UserBooks).ThenInclude(ub => ub.Book)
        .OrderBy(u => u.UserName);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
      return await Get().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByUserNameAsync(string userName)
    {
      return await Get().FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
      return await Get().ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
      var entry = await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
      return entry.Entity;
    }

    public async Task<User> UpdateAsync(User user)
    {
      var existing = await _context.Users.FirstAsync(b => b.Id == user.Id);
      _context.Entry(existing).CurrentValues.SetValues(user);
      await _context.SaveChangesAsync();
      return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
      var entity = await GetByIdAsync(id);
      if (entity == null) return false;

      _context.Users.Remove(entity);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistAsync(Guid id)
    {
      return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> ExistNameAsync(string userName)
    {
      return await _context.Users.AsNoTracking().AnyAsync(u => u.UserName == userName);
    }

    public async Task<bool> ExistEmailAsync(string email)
    {
      return await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email);
    }


    // ----- Books ------ //
    public async Task<UserBook?> GetUserBookAsync(Guid userId, Guid bookId)
    {
      return await _context.UsersBooks
        .AsNoTracking()
        .Include(ub => ub.User).Include(ub => ub.Book)
        .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);
    }

    public async Task<UserBook> AddBookAsync(Guid userId, AddUserBookRequest request)
    {
      var entity = await _context.UsersBooks.AddAsync(UserBookMapper.Map(request, userId));
      await _context.SaveChangesAsync();

      return await _context.UsersBooks
        .Include(ub => ub.User).Include(ub => ub.Book)
        .FirstAsync(ub => ub.BookId == entity.Entity.BookId && ub.UserId == entity.Entity.UserId);
    }

    public async Task<UserBook> UpdateBookAsync(UserBook userBook)
    {
      var existing = await _context.UsersBooks
        .Include(ub => ub.User).Include(ub => ub.Book)
        .FirstAsync(ub => ub.UserId == userBook.UserId && ub.BookId == userBook.BookId);
      _context.Entry(existing).CurrentValues.SetValues(userBook);
      await _context.SaveChangesAsync();
      return existing;
    }

    public async Task<bool> DeleteBookAsync(Guid userId, Guid bookId)
    {
      var entity = await _context.UsersBooks
        .FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);
      if (entity == null) return false;

      _context.UsersBooks.Remove(entity);
      await _context.SaveChangesAsync();
      return true;
    }

    public async Task<bool> ExistUserBookAsync(Guid userId, Guid bookId)
    {
      return await _context.UsersBooks.AsNoTracking().AnyAsync(ub => ub.UserId == userId && ub.BookId == bookId);
    }
  }
}
