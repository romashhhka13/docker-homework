using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShpaginApp.Auth;
using ShpaginApp.Data.Services;
using ShpaginApp.Models.DTOs;


namespace ShpaginApp.Controllers
{
  [Route("api/authors")]
  [Consumes(MediaTypeNames.Application.Json)]
  [Produces(MediaTypeNames.Application.Json)]
  public class AuthorController(AuthorService service) : ControllerBase
  {
    private readonly AuthorService _service = service;

    [HttpGet("all")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<IEnumerable<AuthorItemResponse>>> GetAll()
    {
      var users = await _service.GetAll();
      return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<AuthorResponse>> GetOne(Guid id)
    {
      return await _service.GetOne(id);
    }

    [HttpPost("create")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<AuthorResponse>> Create([FromBody] CreateAuthorRequest request)
    {
      var user = await _service.Create(request);
      return CreatedAtAction(nameof(GetOne), new { id = user.Id }, user);
    }

    [HttpPut("update")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<AuthorResponse>> UpdatePut(
      [FromQuery] Guid id,
      [FromBody] UpdateAuthorPutRequest request)
    {
      return await _service.UpdatePut(id, request);
    }

    [HttpPatch("update")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<AuthorResponse>> UpdatePatch(
      [FromQuery] Guid id,
      [FromBody] UpdateAuthorPatchRequest request)
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
  }
}
