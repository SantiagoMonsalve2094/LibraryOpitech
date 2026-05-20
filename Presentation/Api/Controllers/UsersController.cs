using System.Security.Claims;
using LibraryOpitech.Application.Features.Users.Commands.CreateUser;
using LibraryOpitech.Application.Features.Users.Commands.DeleteUser;
using LibraryOpitech.Application.Features.Users.Commands.UpdateUser;
using LibraryOpitech.Application.Features.Users.Queries.GetUserById;
using LibraryOpitech.Application.Features.Users.Queries.GetUsers;
using LibraryOpitech.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOpitech.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController(
    GetUsersHandler getUsersHandler,
    GetUserByIdHandler getUserByIdHandler,
    CreateUserHandler createUserHandler,
    UpdateUserHandler updateUserHandler,
    DeleteUserHandler deleteUserHandler) : ControllerBase
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var users = await getUsersHandler.Handle(new GetUsersQuery { Page = page, PageSize = pageSize }, ct);
        return Ok(users);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var user = await getUserByIdHandler.Handle(new GetUserByIdQuery(id), ct);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(idValue, out var userId))
            return Unauthorized();

        var user = await getUserByIdHandler.Handle(new GetUserByIdQuery(userId), ct);
        return user is null ? NotFound() : Ok(user);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken ct)
    {
        var user = await createUserHandler.Handle(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserCommand command, CancellationToken ct)
    {
        var user = await updateUserHandler.Handle(id, command, ct);
        return user is null ? NotFound() : Ok(user);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await deleteUserHandler.Handle(new DeleteUserCommand(id), ct);
        return deleted ? NoContent() : NotFound();
    }
}
