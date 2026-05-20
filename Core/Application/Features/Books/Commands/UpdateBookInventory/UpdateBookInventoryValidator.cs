using FluentValidation;

namespace LibraryOpitech.Application.Features.Books.Commands.UpdateBookInventory;

public sealed class UpdateBookInventoryValidator : AbstractValidator<UpdateBookInventoryCommand>
{
    public UpdateBookInventoryValidator()
    {
        RuleFor(x => x.AvailableUnits).GreaterThanOrEqualTo(0);
        RuleFor(x => x.UnavailableUnits).GreaterThanOrEqualTo(0);
    }
}
