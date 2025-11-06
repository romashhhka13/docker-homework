using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShpaginApp.Auth;
using ShpaginApp.Data.Services;
using ShpaginApp.Models.Mapping;
using ShpaginApp.Models.DTOs;


namespace ShpaginApp.Controllers
{
  [Route("api/users")]
  [Consumes(MediaTypeNames.Application.Json)]
  [Produces(MediaTypeNames.Application.Json)]
  public class UserController(UserService service) : ControllerBase
  {
    private readonly UserService _service = service;

    [HttpGet("all")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<IEnumerable<UserItemResponse>>> GetAll()
    {
      var users = await _service.GetAll();
      return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<UserResponse>> GetOne(Guid id)
    {
      return await _service.GetOne(id);
    }

    [HttpPost("create")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest request)
    {
      var user = await _service.Create(UserMapper.Map(request));
      return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
    }

    [HttpPut("update")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<UserResponse>> UpdatePut(
      [FromQuery] Guid id,
      [FromBody] UpdateUserPutRequest request)
    {
      return await _service.UpdatePut(id, request);
    }

    [HttpPatch("update")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<UserResponse>> UpdatePatch(
      [FromQuery] Guid id,
      [FromBody] UpdateUserPatchRequest request)
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

    // ----- Books ------ //
    [HttpGet("{userId}/books/all")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<IEnumerable<UserBookResponse>>> GetBooks(Guid userId)
    {
      var books = await _service.GetBooks(userId);
      return Ok(books);
    }

    [HttpGet("{userId}/books/{bookId}")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<IEnumerable<UserBookResponse>>> GetBook(Guid userId, Guid bookId)
    {
      var books = await _service.GetBook(userId, bookId);
      return Ok(books);
    }

    [HttpPost("{userId}/books/add")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<UserBookResponse>> AddBook(
      Guid userId, [FromBody] AddUserBookRequest request)
    {
      var userBook = await _service.AddBook(userId, request);
      return Ok(userBook);
    }

    [HttpPut("{userId}/books/update")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<UserBookResponse>> UpdatePutBook(
      Guid userId, [FromQuery] Guid bookId, [FromBody] UpdateUserBookPutRequest request)
    {
      var userBook = await _service.UpdateBook(userId, bookId, request);
      return Ok(userBook);
    }

    [HttpPatch("{userId}/books/update")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<UserBookResponse>> UpdatePatchBook(
      Guid userId, [FromQuery] Guid bookId, [FromBody] UpdateUserBookPatchRequest request)
    {
      var userBook = await _service.UpdateBook(userId, bookId, request);
      return Ok(userBook);
    }

    [HttpDelete("{userId}/books/delete")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult> DeleteBook(Guid userId, [FromQuery] Guid bookId)
    {
      await _service.DeleteBook(userId, bookId);
      return NoContent();
    }
  }
}
