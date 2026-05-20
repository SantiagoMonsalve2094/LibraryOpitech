namespace LibraryOpitech.Application.Features.Books.Queries.GetBooks;

public sealed record GetBooksQuery
{
    public string? Title { get; init; }
    public string? Isbn { get; init; }
    public string? Author { get; init; }
    public string? Category { get; init; }
    public bool? Available { get; init; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
