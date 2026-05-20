namespace LibraryOpitech.Application.Features.Books.Commands.CreateBook;

public sealed record CreateBookCommand
{
    public string Title { get; init; } = string.Empty;
    public string Isbn { get; init; } = string.Empty;
    public int PublicationYear { get; init; }
    public string? Description { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public IReadOnlyCollection<string> AuthorNames { get; init; } = [];
    public int Units { get; init; } = 1;
}
