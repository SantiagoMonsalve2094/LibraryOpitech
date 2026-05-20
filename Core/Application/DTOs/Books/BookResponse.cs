namespace LibraryOpitech.Application.DTOs.Books;

public class BookResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int TotalUnits { get; set; }
    public int AvailableUnits { get; set; }
    public IReadOnlyCollection<AuthorResponse> Authors { get; set; } = [];
}
