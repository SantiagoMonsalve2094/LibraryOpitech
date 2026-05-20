namespace LibraryOpitech.Application.Features.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? Role { get; init; }
    public bool? IsActive { get; init; }
}
