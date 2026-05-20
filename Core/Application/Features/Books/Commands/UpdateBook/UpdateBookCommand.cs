namespace LibraryOpitech.Application.Features.Books.Commands.UpdateBook;

public sealed record UpdateBookCommand
{
    public string? Title { get; init; }
    public string? Isbn { get; init; }
    public int? PublicationYear { get; init; }
    public string? Description { get; init; }
    public string? CategoryName { get; init; }
    public IReadOnlyCollection<string>? AuthorNames { get; init; }
}
