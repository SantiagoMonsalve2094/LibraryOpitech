namespace LibraryOpitech.Domain.Entities;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; set; } = [];
    public ICollection<BookUnit> Units { get; set; } = [];
}
