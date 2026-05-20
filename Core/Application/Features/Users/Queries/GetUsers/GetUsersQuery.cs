namespace LibraryOpitech.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
