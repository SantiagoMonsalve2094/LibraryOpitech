using FluentValidation;
using LibraryOpitech.Application.DTOs.Reservations;
using LibraryOpitech.Application.Features.Reservations.Queries.GetReservations;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Reservations.Commands.CreateReservation;

public sealed class CreateReservationHandler(IUnitOfWork uow, IValidator<CreateReservationCommand> validator)
{
    public async Task<ReservationResponse> Handle(CreateReservationCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var user = await uow.Users.GetByIdAsync(command.UserId, ct)
            ?? throw new InvalidOperationException("User not found.");

        if (!user.IsActive)
            throw new InvalidOperationException("User is not active.");

        var book = await uow.Books.GetByIdWithDetailsAsync(command.BookId, ct)
            ?? throw new InvalidOperationException("Book not found.");

        if (await uow.Reservations.HasAvailableUnitsAsync(command.BookId, ct))
            throw new InvalidOperationException("Book has available units and cannot be reserved.");

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            BookId = command.BookId,
            ReservedAt = DateTime.UtcNow,
            Status = ReservationStatus.Pending,
            User = user,
            Book = book
        };

        await uow.Reservations.AddAsync(reservation, ct);
        await uow.SaveChangesAsync(ct);

        return GetReservationsHandler.ToResponse(reservation);
    }
}
