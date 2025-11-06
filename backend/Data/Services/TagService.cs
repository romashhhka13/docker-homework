using ShpaginApp.Models.DTOs;
using ShpaginApp.Exceptions;
using ShpaginApp.Data.Repositories;
using ShpaginApp.Models.Mapping;

namespace ShpaginApp.Data.Services
{
  public class TagService(TagRepository tagRepo)
  {
    private readonly TagRepository _tagRepo = tagRepo;

    public async Task<IEnumerable<TagResponse>> GetAll()
    {
      return (await _tagRepo.GetAllAsync()).Select(TagMapper.Map);
    }

    public async Task<TagResponse> GetOne(Guid id)
    {
      var tag = await _tagRepo.GetByIdAsync(id);
      return tag is null
        ? throw new AppException(StatusCodes.Status404NotFound, $"Tag with id << {id} >> not found")
        : TagMapper.Map(tag);
    }

    public async Task<TagResponse> Create(CreateTagRequest request)
    {
      var tag = await _tagRepo.CreateAsync(TagMapper.Map(request));
      return TagMapper.Map(tag);
    }

    public async Task<TagResponse> Update(Guid id, UpdateTagRequest request)
    {
      var tag = await _tagRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"Tag with id << {id} >> not found");

      tag.ApplyUpdate(request);
      tag = await _tagRepo.UpdateAsync(tag);
      return TagMapper.Map(tag);
    }

    public async Task Delete(Guid id)
    {
      if (!await _tagRepo.ExistAsync(id))
        throw new AppException(StatusCodes.Status404NotFound, $"Tag with id << {id} >> not found");

      await _tagRepo.DeleteAsync(id);
    }
  }
}
