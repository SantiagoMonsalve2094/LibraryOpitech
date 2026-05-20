using FluentValidation;
using LibraryOpitech.Application.DTOs.Reservations;
using LibraryOpitech.Application.Features.Reservations.Queries.GetReservations;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Reservations.Commands.CancelReservation;

public sealed class CancelReservationHandler(IUnitOfWork uow, IValidator<CancelReservationCommand> validator)
{
    public async Task<ReservationResponse?> Handle(CancelReservationCommand command, Guid currentUserId, bool isAdmin, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var reservation = await uow.Reservations.GetByIdWithDetailsAsync(command.ReservationId, ct);
        if (reservation is null) return null;

        if (!isAdmin && reservation.UserId != currentUserId)
            throw new InvalidOperationException("You can only cancel your own reservations.");

        if (reservation.Status == ReservationStatus.Cancelled)
            throw new InvalidOperationException("Reservation is already cancelled.");

        reservation.Status = ReservationStatus.Cancelled;
        uow.Reservations.Update(reservation);
        await uow.SaveChangesAsync(ct);

        return GetReservationsHandler.ToResponse(reservation);
    }
}
