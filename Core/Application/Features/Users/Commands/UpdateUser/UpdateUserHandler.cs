using FluentValidation;
using LibraryOpitech.Application.DTOs.Users;
using LibraryOpitech.Application.Features.Users.Commands.CreateUser;
using LibraryOpitech.Application.Features.Users.Queries.GetUsers;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserHandler(IUnitOfWork uow, IPasswordHasher passwordHasher, IValidator<UpdateUserCommand> validator)
{
    public async Task<UserResponse?> Handle(Guid id, UpdateUserCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var user = await uow.Users.GetByIdAsync(id, ct);
        if (user is null) return null;

        if (command.Email is not null && await uow.Users.EmailExistsAsync(command.Email.Trim(), id, ct))
            throw new InvalidOperationException("A user with that email already exists.");

        if (command.Name is not null)
            user.Name = command.Name.Trim();

        if (command.Email is not null)
            user.Email = command.Email.Trim().ToLowerInvariant();

        if (command.Password is not null)
            user.PasswordHash = passwordHasher.Hash(command.Password);

        if (command.Role is not null)
            user.Role = UserRules.ParseRole(command.Role);

        if (command.IsActive is not null)
            user.IsActive = command.IsActive.Value;

        uow.Users.Update(user);
        await uow.SaveChangesAsync(ct);

        return GetUsersHandler.ToResponse(user);
    }
}
