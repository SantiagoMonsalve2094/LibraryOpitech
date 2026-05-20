using System.Security.Claims;
using LibraryOpitech.Application.Features.Reservations.Commands.CancelReservation;
using LibraryOpitech.Application.Features.Reservations.Commands.CreateReservation;
using LibraryOpitech.Application.Features.Reservations.Queries.GetReservations;
using LibraryOpitech.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOpitech.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ReservationsController(
    CreateReservationHandler createReservationHandler,
    CancelReservationHandler cancelReservationHandler,
    GetReservationsHandler getReservationsHandler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationCommand command, CancellationToken ct)
    {
        var userId = GetCurrentUserId();
        var isAdmin = User.IsInRole(nameof(UserRole.Admin));
        var reservation = await createReservationHandler.Handle(command with { UserId = isAdmin ? command.UserId : userId }, ct);

        return Created(string.Empty, reservation);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var reservation = await cancelReservationHandler.Handle(new CancelReservationCommand(id), GetCurrentUserId(), User.IsInRole(nameof(UserRole.Admin)), ct);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMine([FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var reservations = await getReservationsHandler.Handle(new GetReservationsQuery { UserId = GetCurrentUserId(), Status = status, Page = page, PageSize = pageSize }, ct);
        return Ok(reservations);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? userId, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var reservations = await getReservationsHandler.Handle(new GetReservationsQuery { UserId = userId, Status = status, Page = page, PageSize = pageSize }, ct);
        return Ok(reservations);
    }

    private Guid GetCurrentUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var userId) ? userId : throw new InvalidOperationException("Invalid user token.");
    }
}
