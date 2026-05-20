using FluentValidation;
using LibraryOpitech.Application.DTOs.Auth;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Auth.Commands.Login;

public sealed class LoginHandler(
    IUnitOfWork uow,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IValidator<LoginCommand> validator)
{
    public async Task<AuthResponse> Handle(LoginCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var user = await uow.Users.GetByEmailAsync(command.Email.Trim(), ct);

        if (user is null || !user.IsActive || !passwordHasher.Verify(command.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid credentials.");

        return jwtTokenGenerator.Generate(user);
    }
}
