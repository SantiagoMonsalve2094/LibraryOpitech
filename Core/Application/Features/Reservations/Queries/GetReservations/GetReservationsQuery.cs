namespace LibraryOpitech.Application.Features.Reservations.Queries.GetReservations;

public sealed record GetReservationsQuery
{
    public Guid? UserId { get; init; }
    public string? Status { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
