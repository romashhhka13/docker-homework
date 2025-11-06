using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using ShpaginApp.Data.Services;
using ShpaginApp.Models.Mapping;
using ShpaginApp.Models.DTOs;


namespace ShpaginApp.Controllers
{
  [Route("api/auth")]
  [Consumes(MediaTypeNames.Application.Json)]
  [Produces(MediaTypeNames.Application.Json)]
  public class AuthController(AuthService authService) : ControllerBase
  {
    private readonly AuthService _authService = authService;

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
      var user = await _authService.Register(AuthMapper.Map(request));
      return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
      var token = await _authService.Login(AuthMapper.Map(request));
      return Ok(new LoginResponse(AccessToken: token));
    }
  }
}
