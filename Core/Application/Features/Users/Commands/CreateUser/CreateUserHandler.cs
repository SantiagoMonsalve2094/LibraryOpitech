using FluentValidation;
using LibraryOpitech.Application.DTOs.Users;
using LibraryOpitech.Application.Features.Users.Queries.GetUsers;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserHandler(IUnitOfWork uow, IPasswordHasher passwordHasher, IValidator<CreateUserCommand> validator)
{
    public async Task<UserResponse> Handle(CreateUserCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        if (await uow.Users.EmailExistsAsync(command.Email.Trim(), null, ct))
            throw new InvalidOperationException("A user with that email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = command.Name.Trim(),
            Email = command.Email.Trim().ToLowerInvariant(),
            PasswordHash = passwordHasher.Hash(command.Password),
            Role = UserRules.ParseRole(command.Role),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await uow.Users.AddAsync(user, ct);
        await uow.SaveChangesAsync(ct);

        return GetUsersHandler.ToResponse(user);
    }
}

internal static class UserRules
{
    internal static UserRole ParseRole(string role)
    {
        return Enum.TryParse<UserRole>(role, true, out var parsedRole)
            ? parsedRole
            : throw new InvalidOperationException("Role must be Admin or User.");
    }
}
