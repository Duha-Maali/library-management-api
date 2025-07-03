using LibraryManagement.Business.IServices;
using LibraryManagement.Business.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Presentation.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginVM request)
    {
        var token = await _userService.LoginAsync(request);
        if (token == null)
            return Unauthorized("Invalid username or password.");

        return Ok(new { Token = token });
    }
}
