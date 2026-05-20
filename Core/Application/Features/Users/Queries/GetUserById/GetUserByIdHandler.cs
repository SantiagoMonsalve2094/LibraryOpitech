using FluentValidation;
using LibraryOpitech.Application.DTOs.Users;
using LibraryOpitech.Application.Features.Users.Queries.GetUsers;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdHandler(IUnitOfWork uow, IValidator<GetUserByIdQuery> validator)
{
    public async Task<UserResponse?> Handle(GetUserByIdQuery query, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(query, ct);

        var user = await uow.Users.GetByIdAsync(query.Id, ct);
        return user is null ? null : GetUsersHandler.ToResponse(user);
    }
}
