namespace LibraryOpitech.Domain.Entities;

public class Author
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<BookAuthor> BookAuthors { get; set; } = [];
}
