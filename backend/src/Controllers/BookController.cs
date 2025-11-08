using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShpaginApp.Auth;
using ShpaginApp.Data.Services;
using ShpaginApp.Helpers.Pagination;
using ShpaginApp.Models;
using ShpaginApp.Models.DTOs;


namespace ShpaginApp.Controllers
{
  [Route("api/books")]
  [Consumes(MediaTypeNames.Application.Json)]
  [Produces(MediaTypeNames.Application.Json)]
  public class BookController(BookService service) : ControllerBase
  {
    private readonly BookService _service = service;

    [HttpGet("all")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<IEnumerable<BookItemResponse>>> GetAll()
    {
      return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<BookResponse>> GetOne(
      Guid id,
      [FromQuery] bool statistics,
      [FromQuery] BookIncludeOptions? include)
    {
      return Ok(await _service.GetOne(id, include, statistics));
    }

    [HttpPost("create")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<BookResponse>> Create([FromBody] CreateBookRequest request)
    {
      var user = await _service.Create(request);
      return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
    }

    [HttpPut("update")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<BookResponse>> UpdatePut(
      [FromQuery] Guid id,
      [FromBody] UpdateBookPutRequest request)
    {
      return await _service.UpdatePut(id, request);
    }

    [HttpPatch("update")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<BookResponse>> UpdatePatch(
      [FromQuery] Guid id,
      [FromBody] UpdateBookPatchRequest request)
    {
      return await _service.UpdatePatch(id, request);
    }

    [HttpDelete("delete")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult> Delete(Guid id)
    {
      await _service.Delete(id);
      return NoContent();
    }

    [HttpGet("search")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult> Search(
      [FromQuery] BookSearchRequest searchRequest,
      [FromQuery] BookIncludeOptions? include,
      [FromQuery] PaginationRequest paginationRequest)
    {
      if (paginationRequest.Page == null && paginationRequest.PageSize == null)
      {
        return Ok(await _service.SearchBooksAsync(searchRequest, include));
      }
      else if (paginationRequest.Page != null && paginationRequest.PageSize != null)
      {
        return Ok(await _service.SearchBooksAsync(searchRequest, include, paginationRequest));
      }
      else
      {
        return BadRequest("Missing one or more required query parameters");
      }
    }
  }
}
