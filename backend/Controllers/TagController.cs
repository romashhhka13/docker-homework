using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShpaginApp.Auth;
using ShpaginApp.Data.Services;
using ShpaginApp.Models.DTOs;


namespace ShpaginApp.Controllers
{
  [Route("api/tags")]
  [Consumes(MediaTypeNames.Application.Json)]
  [Produces(MediaTypeNames.Application.Json)]
  public class TagController(TagService service) : ControllerBase
  {
    private readonly TagService _service = service;

    [HttpGet("all")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<IEnumerable<TagResponse>>> GetAll()
    {
      return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = $"{AuthRoles.Admin},{AuthRoles.User}")]
    public async Task<ActionResult<TagResponse>> GetOne(Guid id)
    {
      return await _service.GetOne(id);
    }

    [HttpPost("create")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<TagResponse>> Create([FromBody] CreateTagRequest request)
    {
      var tag = await _service.Create(request);
      return CreatedAtAction(nameof(GetOne), new { id = tag.Id }, tag);
    }

    [HttpPut("update")]
    [Authorize(Roles = AuthRoles.Admin)]
    public async Task<ActionResult<TagResponse>> Update([FromQuery] Guid id, [FromBody] UpdateTagRequest request)
    {
      return await _service.Update(id, request);
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
