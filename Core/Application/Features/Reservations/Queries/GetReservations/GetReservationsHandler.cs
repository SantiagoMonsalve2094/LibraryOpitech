using FluentValidation;
using LibraryOpitech.Application.Common;
using LibraryOpitech.Application.DTOs.Reservations;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Features.Reservations.Queries.GetReservations;

public sealed class GetReservationsHandler(IUnitOfWork uow, IValidator<GetReservationsQuery> validator)
{
    public async Task<PagedResult<ReservationResponse>> Handle(GetReservationsQuery query, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(query, ct);

        var (items, totalCount) = await uow.Reservations.SearchAsync(query.UserId, query.Status, query.Page, query.PageSize, ct);

        return new PagedResult<ReservationResponse>
        {
            Items = items.Select(ToResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public static ReservationResponse ToResponse(Reservation reservation)
    {
        return new ReservationResponse
        {
            Id = reservation.Id,
            UserId = reservation.UserId,
            UserName = reservation.User?.Name ?? string.Empty,
            BookId = reservation.BookId,
            BookTitle = reservation.Book?.Title ?? string.Empty,
            ReservedAt = reservation.ReservedAt,
            Status = reservation.Status.ToString()
        };
    }
}
