using ShpaginApp.Models.DTOs;
using ShpaginApp.Exceptions;
using ShpaginApp.Data.Repositories;
using ShpaginApp.Models.Mapping;

namespace ShpaginApp.Data.Services
{
  public class UserService(UserRepository userRepo, BookRepository bookRepo)
  {
    private readonly UserRepository _userRepo = userRepo;
    private readonly BookRepository _bookRepo = bookRepo;

    public async Task<IEnumerable<UserItemResponse>> GetAll()
    {
      var users = await _userRepo.GetAllAsync();
      return users.Select(UserMapper.MapItem).OrderBy(u => u.UserName);
    }

    public async Task<UserResponse> GetOne(Guid id)
    {
      var user = await _userRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"User with id << {id} >> not found");

      return UserMapper.Map(user);
    }

    public async Task<UserEntity> GetEntityByUserName(string userName)
    {
      var user = await _userRepo.GetByUserNameAsync(userName);
      return user is null
        ? throw new AppException(StatusCodes.Status404NotFound, $"User with username << {userName} >> not found")
        : UserMapper.MapToEntity(user);
    }

    public async Task<UserResponse> Create(CreateUserCommand command)
    {
      if (await _userRepo.ExistNameAsync(command.UserName))
        throw new AppException(StatusCodes.Status400BadRequest,
          $"User with username << {command.UserName} >> already exist");

      if (await _userRepo.ExistEmailAsync(command.Email))
        throw new AppException(StatusCodes.Status400BadRequest,
          $"User with email << {command.Email} >> already exist");

      var user = await _userRepo.CreateAsync(UserMapper.Map(command));
      return UserMapper.Map(user);
    }

    public async Task<UserResponse> UpdatePut(Guid id, UpdateUserPutRequest request)
    {
      var user = await _userRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"User with id << {id} >> not found");

      user.ApplyUpdate(request);
      user = await _userRepo.UpdateAsync(user);
      return UserMapper.Map(user);
    }

    public async Task<UserResponse> UpdatePatch(Guid id, UpdateUserPatchRequest request)
    {
      var user = await _userRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"User with id << {id} >> not found");

      user.ApplyUpdate(request);
      user = await _userRepo.UpdateAsync(user);
      return UserMapper.Map(user);
    }

    public async Task Delete(Guid id)
    {
      if (!await _userRepo.ExistAsync(id))
        throw new AppException(StatusCodes.Status404NotFound, $"User with id << {id} >> not found");

      await _userRepo.DeleteAsync(id);
    }


    // ----- Books ------ //
    public async Task<IEnumerable<UserBookResponse>> GetBooks(Guid userId)
    {
      var user = await _userRepo.GetByIdAsync(userId)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"User with id << {userId} >> not found");

      return user.UserBooks.Select(UserBookMapper.Map).OrderByDescending(b => b.AddedAt).ThenBy(b => b.Name);
    }

    public async Task<UserBookResponse> GetBook(Guid userId, Guid bookId)
    {
      if (!await _userRepo.ExistAsync(userId))
        throw new AppException(StatusCodes.Status404NotFound, $"User with id << {userId} >> not found");

      if (!await _bookRepo.ExistAsync(bookId))
        throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {bookId} >> not found");

      var userBook = await _userRepo.GetUserBookAsync(userId, bookId)
        ?? throw new AppException(StatusCodes.Status400BadRequest,
          $"Relation `User with id << {userId} >> and Book with id << {bookId} >>` not found");

      return UserBookMapper.Map(userBook);
    }

    public async Task<UserBookResponse> AddBook(Guid userId, AddUserBookRequest request)
    {
      if (!await _userRepo.ExistAsync(userId))
        throw new AppException(StatusCodes.Status404NotFound, $"User with id << {userId} >> not found");

      if (!await _bookRepo.ExistAsync(request.BookId))
        throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {request.BookId} >> not found");

      if (await _userRepo.ExistUserBookAsync(userId, request.BookId))
        throw new AppException(StatusCodes.Status400BadRequest,
          $"Relation `User with id << {userId} >> and Book with id << {request.BookId} >>` already exist");

      var userBook = await _userRepo.AddBookAsync(userId, request);
      return UserBookMapper.Map(userBook);
    }

    public async Task<UserBookResponse> UpdateBook(Guid userId, Guid bookId, UpdateUserBookPutRequest request)
    {
      if (!await _userRepo.ExistAsync(userId))
        throw new AppException(StatusCodes.Status404NotFound, $"User with id << {userId} >> not found");

      if (!await _bookRepo.ExistAsync(bookId))
        throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {bookId} >> not found");

      var userBook = await _userRepo.GetUserBookAsync(userId, bookId)
        ?? throw new AppException(StatusCodes.Status400BadRequest,
          $"Relation `User with id << {userId} >> and Book with id << {bookId} >>` not found");

      userBook.ApplyUpdate(request);
      userBook = await _userRepo.UpdateBookAsync(userBook);
      return UserBookMapper.Map(userBook);
    }

    public async Task<UserBookResponse> UpdateBook(Guid userId, Guid bookId, UpdateUserBookPatchRequest request)
    {
      if (!await _userRepo.ExistAsync(userId))
        throw new AppException(StatusCodes.Status404NotFound, $"User with id << {userId} >> not found");

      if (!await _bookRepo.ExistAsync(bookId))
        throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {bookId} >> not found");

      var userBook = await _userRepo.GetUserBookAsync(userId, bookId)
        ?? throw new AppException(StatusCodes.Status400BadRequest,
          $"Relation `User with id << {userId} >> and Book with id << {bookId} >>` not found");

      userBook.ApplyUpdate(request);
      userBook = await _userRepo.UpdateBookAsync(userBook);
      return UserBookMapper.Map(userBook);
    }

    public async Task<bool> DeleteBook(Guid userId, Guid bookId)
    {
      if (!await _userRepo.ExistAsync(userId))
        throw new AppException(StatusCodes.Status404NotFound, $"User with id << {userId} >> not found");

      if (!await _bookRepo.ExistAsync(bookId))
        throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {bookId} >> not found");

      if (!await _userRepo.ExistUserBookAsync(userId, bookId))
        throw new AppException(StatusCodes.Status404NotFound,
          $"Relation `User with id << {userId} >> and Book with id << {bookId} >>` not found");

      return await _userRepo.DeleteBookAsync(userId, bookId);
    }
  }
}
