using LibraryOpitech.Application.Features.Auth.Commands.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOpitech.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(LoginHandler loginHandler) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var response = await loginHandler.Handle(command, ct);
        return Ok(response);
    }
}
