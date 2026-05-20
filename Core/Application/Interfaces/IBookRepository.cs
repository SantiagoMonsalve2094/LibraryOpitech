using LibraryOpitech.Application.Features.Books.Queries.GetBooks;
using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Interfaces;

public interface IBookRepository : IRepository<Book>
{
    Task<Book?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<(IReadOnlyCollection<Book> Items, int TotalCount)> SearchAsync(GetBooksQuery query, CancellationToken ct = default);
    Task<bool> IsbnExistsAsync(string isbn, Guid? excludedId = null, CancellationToken ct = default);
    Task<Category?> GetCategoryByNameAsync(string name, CancellationToken ct = default);
    Task AddCategoryAsync(Category category, CancellationToken ct = default);
    Task<IReadOnlyCollection<Author>> GetAuthorsByNamesAsync(IReadOnlyCollection<string> names, CancellationToken ct = default);
    Task AddAuthorAsync(Author author, CancellationToken ct = default);
    Task ReplaceUnitsAsync(Book book, IReadOnlyCollection<BookUnit> units, CancellationToken ct = default);
}
