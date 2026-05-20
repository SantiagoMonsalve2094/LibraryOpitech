namespace LibraryOpitech.Application.Features.Reservations.Commands.CreateReservation;

public sealed record CreateReservationCommand
{
    public Guid UserId { get; init; }
    public Guid BookId { get; init; }
}
