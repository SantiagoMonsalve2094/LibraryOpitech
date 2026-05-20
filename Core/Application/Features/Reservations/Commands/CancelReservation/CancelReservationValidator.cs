using FluentValidation;

namespace LibraryOpitech.Application.Features.Reservations.Commands.CancelReservation;

public sealed class CancelReservationValidator : AbstractValidator<CancelReservationCommand>
{
    public CancelReservationValidator()
    {
        RuleFor(x => x.ReservationId).NotEmpty();
    }
}
