using FluentValidation;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(150);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.Role)
            .Must(role => Enum.TryParse<UserRole>(role, true, out _))
            .WithMessage("Role must be Admin or User.");
    }
}
