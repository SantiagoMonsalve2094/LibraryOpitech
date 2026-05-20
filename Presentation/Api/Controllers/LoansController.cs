using System.Security.Claims;
using LibraryOpitech.Application.Features.Loans.Commands.CreateLoan;
using LibraryOpitech.Application.Features.Loans.Commands.ReturnLoan;
using LibraryOpitech.Application.Features.Loans.Queries.GetLoans;
using LibraryOpitech.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOpitech.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LoansController(
    CreateLoanHandler createLoanHandler,
    ReturnLoanHandler returnLoanHandler,
    GetLoansHandler getLoansHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLoanCommand command, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var isAdmin = User.IsInRole(nameof(UserRole.Admin));
        var loan = await createLoanHandler.Handle(command with { UserId = isAdmin ? command.UserId : userId }, ct);

        return Created(string.Empty, loan);
    }

    [HttpPatch("{id:guid}/return")]
    public async Task<IActionResult> Return(Guid id, CancellationToken ct)
    {
        var loan = await returnLoanHandler.Handle(new ReturnLoanCommand(id), GetCurrentUserId(), User.IsInRole(nameof(UserRole.Admin)), ct);
        return loan is null ? NotFound() : Ok(loan);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMine([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var loans = await getLoansHandler.Handle(new GetLoansQuery { UserId = GetCurrentUserId(), Status = status, Page = page, PageSize = pageSize }, ct);
        return Ok(loans);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? userId, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var loans = await getLoansHandler.Handle(new GetLoansQuery { UserId = userId, Status = status, Page = page, PageSize = pageSize }, ct);
        return Ok(loans);
    }

    private Guid GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var userId) ? userId : throw new InvalidOperationException("Invalid user token.");
    }
}
