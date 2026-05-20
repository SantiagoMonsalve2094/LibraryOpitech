namespace LibraryOpitech.Application.Features.Users.Commands.CreateUser;

public sealed record CreateUserCommand
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Role { get; init; } = "User";
}
