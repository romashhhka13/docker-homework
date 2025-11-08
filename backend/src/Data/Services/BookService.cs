using ShpaginApp.Models.DTOs;
using ShpaginApp.Exceptions;
using ShpaginApp.Models.Entities;
using ShpaginApp.Data.Repositories;
using ShpaginApp.Models.Mapping;
using ShpaginApp.Helpers.Pagination;
using Microsoft.EntityFrameworkCore;
using ShpaginApp.Models;

namespace ShpaginApp.Data.Services
{
  public class BookService(BookRepository bookRepo)
  {
    private readonly BookRepository _bookRepo = bookRepo;

    public async Task<IEnumerable<BookItemResponse>> GetAll()
    {
      var books = await _bookRepo.GetAllAsync();
      return books.Select(BookMapper.MapItem);
    }

    public async Task<PaginatedResult<BookItemResponse>> GetAll(PaginationRequest request)
    {
      var query = _bookRepo.Get();
      var books = await PaginatedResult<Book>.CreateAsync(query, request);

      return books.MapItems(BookMapper.MapItem);
    }

    public async Task<BookResponse> GetOne(Guid id)
    {
      var book = await _bookRepo.GetByIdAsync(id);
      return book is null
        ? throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {id} >> not found")
        : BookMapper.Map(book);
    }

    private static BookResponse MaskBookResponse(
      BookResponse response,
      BookIncludeOptions? includeOptions = null,
      BookStatistics? statistics = null)
    {
      if (includeOptions == null || !includeOptions.Value.HasFlag(BookIncludeOptions.Details))
        response = response with
        {
          Year = null,
          AgeRating = null,
          Publisher = null,
          Annotation = null,
          CreatedAt = null,
          UpdatedAt = null,
        };

      response = (includeOptions != null && includeOptions.Value.HasFlag(BookIncludeOptions.Tags))
        ? response with { Tags = response.Tags!.OrderBy(t => t.Value) }
        : response with { Tags = null };

      if (statistics != null)
        response = response with { Statistics = statistics };

      return response;
    }

    public async Task<BookResponse> GetOne(Guid id, BookIncludeOptions? include, bool statistics)
    {
      var book = await _bookRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {id} >> not found");

      var response = BookMapper.Map(book);
      return MaskBookResponse(response, include, statistics ? await _bookRepo.GetStatisticsByIdAsync(id) : null);
    }

    public async Task<IEnumerable<BookResponse>> SearchBooksAsync(
      BookSearchRequest searchRequest,
      BookIncludeOptions? include)
    {
      var books = await _bookRepo.SearchBooksQuery(searchRequest).ToListAsync();
      return books.Select(book => MaskBookResponse(BookMapper.Map(book), include, null));
    }

    public async Task<PaginatedResult<BookResponse>> SearchBooksAsync(
      BookSearchRequest searchRequest,
      BookIncludeOptions? include,
      PaginationRequest paginationRequest)
    {
      var query = _bookRepo.SearchBooksQuery(searchRequest);
      var books = await PaginatedResult<BookResponse>.CreateAsync(query, paginationRequest);
      return books.MapItems(book => MaskBookResponse(BookMapper.Map(book), include, null));
    }

    public async Task<BookResponse> Create(CreateBookRequest request)
    {
      if (!request.AuthorIds.Any())
        throw new AppException(StatusCodes.Status400BadRequest, "At least one author is required");

      if (request.TagIds != null && !request.TagIds.Any())
        throw new AppException(StatusCodes.Status400BadRequest, "At least one tag is required");

      var book = await _bookRepo.CreateAsync(BookMapper.Map(request), request.AuthorIds, request.TagIds);
      return BookMapper.Map(book);
    }

    public async Task<BookResponse> UpdatePut(Guid id, UpdateBookPutRequest request)
    {
      var book = await _bookRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {id} >> not found");

      if (!request.AuthorIds.Any())
        throw new AppException(StatusCodes.Status400BadRequest, "At least one author is required");

      book.ApplyUpdate(request);
      book = await _bookRepo.UpdateAsync(book, request.AuthorIds, request.TagIds);
      return BookMapper.Map(book);
    }

    public async Task<BookResponse> UpdatePatch(Guid id, UpdateBookPatchRequest request)
    {
      var book = await _bookRepo.GetByIdAsync(id)
        ?? throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {id} >> not found");

      if (request.AuthorIds != null && !request.AuthorIds.Any())
        throw new AppException(StatusCodes.Status400BadRequest, "At least one author is required");

      book.ApplyUpdate(request);
      book = await _bookRepo.UpdateAsync(book, request.AuthorIds, request.TagIds);
      return BookMapper.Map(book);
    }

    public async Task Delete(Guid id)
    {
      if (!await _bookRepo.ExistAsync(id))
        throw new AppException(StatusCodes.Status404NotFound, $"Book with id << {id} >> not found");

      await _bookRepo.DeleteAsync(id);
    }
  }
}
