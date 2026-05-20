using FluentValidation;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Reservations.Queries.GetReservations;

public sealed class GetReservationsValidator : AbstractValidator<GetReservationsQuery>
{
    public GetReservationsValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
        RuleFor(x => x.Status)
            .Must(status => status is null || Enum.TryParse<ReservationStatus>(status, true, out _))
            .WithMessage("Status must be Pending or Cancelled.");
    }
}
