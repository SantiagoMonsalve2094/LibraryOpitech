using FluentValidation;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserHandler(IUnitOfWork uow, IValidator<DeleteUserCommand> validator)
{
    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var user = await uow.Users.GetByIdAsync(command.Id, ct);
        if (user is null) return false;

        uow.Users.Delete(user);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
