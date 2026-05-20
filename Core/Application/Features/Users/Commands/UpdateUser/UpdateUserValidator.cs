using FluentValidation;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150)
            .When(x => x.Name is not null);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150)
            .When(x => x.Email is not null);

        RuleFor(x => x.Password)
            .MinimumLength(6)
            .When(x => x.Password is not null);

        RuleFor(x => x.Role)
            .Must(role => role is null || Enum.TryParse<UserRole>(role, true, out _))
            .WithMessage("Role must be Admin or User.");
    }
}
