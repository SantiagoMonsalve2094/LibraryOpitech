using FluentValidation;
using LibraryOpitech.Application.Common;
using LibraryOpitech.Application.DTOs.Users;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersHandler(IUnitOfWork uow, IValidator<GetUsersQuery> validator)
{
    public async Task<PagedResult<UserResponse>> Handle(GetUsersQuery query, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(query, ct);

        var (items, totalCount) = await uow.Users.SearchAsync(query.Page, query.PageSize, ct);

        return new PagedResult<UserResponse>
        {
            Items = items.Select(ToResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public static UserResponse ToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}
