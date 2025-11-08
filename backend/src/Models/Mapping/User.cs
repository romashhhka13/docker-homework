using Riok.Mapperly.Abstractions;
using ShpaginApp.Helpers;
using ShpaginApp.Models.Entities;
using ShpaginApp.Models.DTOs;

namespace ShpaginApp.Models.Mapping
{
  [Mapper]
  public static partial class UserBookMapper
  {
    [MapProperty(nameof(UserBook.BookId), nameof(UserBookResponse.Id))]
    [MapProperty(nameof(UserBook.Book.Name), nameof(UserBookResponse.Name))]
    [MapperIgnoreSource(nameof(UserBook.UserId))]
    [MapperIgnoreSource(nameof(UserBook.User))]
    public static partial UserBookResponse Map(UserBook userBook);

    [MapperIgnoreTarget(nameof(UserBook.User))]
    [MapperIgnoreTarget(nameof(UserBook.Book))]
    public static partial UserBook Map(AddUserBookRequest request, Guid userId);

    [MapperIgnoreTarget(nameof(UserBook.UserId))]
    [MapperIgnoreTarget(nameof(UserBook.BookId))]
    [MapperIgnoreTarget(nameof(UserBook.User))]
    [MapperIgnoreTarget(nameof(UserBook.Book))]
    public static partial void ApplyUpdate([MappingTarget] this UserBook userBook, UpdateUserBookPutRequest request);

    [MapperIgnoreTarget(nameof(UserBook.UserId))]
    [MapperIgnoreTarget(nameof(UserBook.BookId))]
    [MapperIgnoreTarget(nameof(UserBook.User))]
    [MapperIgnoreTarget(nameof(UserBook.Book))]
    public static partial void ApplyUpdate([MappingTarget] this UserBook userBook, UpdateUserBookPatchRequest request);
  }

  [Mapper]
  [UseStaticMapper(typeof(BookMapper))]
  [UseStaticMapper(typeof(UserBookMapper))]
  public static partial class UserMapper
  {
    [MapperIgnoreSource(nameof(User.UserBooks))]
    public static partial UserEntity MapToEntity(User user);

    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.UserBooks))]
    public static partial UserResponse Map(User user);

    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.DisplayName))]
    [MapperIgnoreSource(nameof(User.BirthDate))]
    [MapperIgnoreSource(nameof(User.CreatedAt))]
    [MapperIgnoreSource(nameof(User.UpdatedAt))]
    [MapperIgnoreSource(nameof(User.UserBooks))]
    public static partial UserItemResponse MapItem(User user);

    [MapProperty(nameof(CreateUserRequest.Password), nameof(CreateUserCommand.PasswordHash), Use = nameof(GetPasswordHash))]
    public static partial CreateUserCommand Map(CreateUserRequest request);

    [UserMapping(Default = false)]
    private static string GetPasswordHash(string password) => PasswordHelper.HashPassword(password);

    [MapperIgnoreTarget(nameof(User.Id))]
    [MapperIgnoreTarget(nameof(User.UserBooks))]
    public static partial User Map(CreateUserCommand command);

    [MapperIgnoreTarget(nameof(User.Id))]
    [MapperIgnoreTarget(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(UpdateUserPatchRequest.Password))]
    private static partial void _ApplyUpdate([MappingTarget] this User user, UpdateUserPutRequest request);

    public static void ApplyUpdate([MappingTarget] this User user, UpdateUserPutRequest request)
    {
      user._ApplyUpdate(request);
      if (request.Password != null)
        user.PasswordHash = PasswordHelper.HashPassword(request.Password);
    }

    [MapperIgnoreTarget(nameof(User.Id))]
    [MapperIgnoreTarget(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(UpdateUserPatchRequest.Password))]
    private static partial void _ApplyUpdate([MappingTarget] this User user, UpdateUserPatchRequest request);

    public static void ApplyUpdate([MappingTarget] this User user, UpdateUserPatchRequest request)
    {
      user._ApplyUpdate(request);
      if (request.Password != null)
        user.PasswordHash = PasswordHelper.HashPassword(request.Password);
    }
  }
}
