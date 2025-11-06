using ShpaginApp.Models.DTOs;
using ShpaginApp.Exceptions;
using ShpaginApp.Models.Mapping;
using ShpaginApp.Data.Repositories;

namespace ShpaginApp.Data.Services
{
  public class AuthorService(AuthorRepository authorRepo)
  {
    private readonly AuthorRepository _authorRepo = authorRepo;

    public async Task<IEnumerable<AuthorItemResponse>> GetAll()
    {
      return (await _authorRepo.GetAllAsync()).Select(AuthorMapper.MapItem);
    }

    public async Task<AuthorResponse> GetOne(Guid id)
    {
      var tag = await _authorRepo.GetByIdAsync(id);
      return tag is null
        ? throw new AppException(StatusCodes.Status404NotFound, $"Author with id << {id} >> not found")
        : AuthorMapper.Map(tag);
    }

    public async Task<AuthorResponse> Create(CreateAuthorRequest request)
    {
      var tag = await _authorRepo.CreateAsync(AuthorMapper.Map(request));
      return AuthorMapper.Map(tag);
    }

    public async Task<AuthorResponse> UpdatePut(Guid id, UpdateAuthorPutRequest request)
    {
      var author = await _authorRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"Author with id << {id} >> not found");

      author.ApplyUpdate(request);
      author = await _authorRepo.UpdateAsync(author);
      return AuthorMapper.Map(author);
    }

    public async Task<AuthorResponse> UpdatePatch(Guid id, UpdateAuthorPatchRequest request)
    {
      var author = await _authorRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"Author with id << {id} >> not found");

      author.ApplyUpdate(request);
      author = await _authorRepo.UpdateAsync(author);
      return AuthorMapper.Map(author);
    }

    public async Task Delete(Guid id)
    {
      if (!await _authorRepo.ExistAsync(id))
        throw new AppException(StatusCodes.Status404NotFound, $"Author with id << {id} >> not found");

      await _authorRepo.DeleteAsync(id);
    }
  }
}
