using LibraryOpitech.Application.Features.Books.Queries.GetBooks;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryOpitech.Infrastructure.Persistence.Repositories;

public class BookRepository(LibraryOpitechDbContext context) : Repository<Book>(context), IBookRepository
{
    public async Task<Book?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await Context.Books
            .Include(x => x.Category)
            .Include(x => x.BookAuthors)
            .ThenInclude(x => x.Author)
            .Include(x => x.Units)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<(IReadOnlyCollection<Book> Items, int TotalCount)> SearchAsync(GetBooksQuery query, CancellationToken ct = default)
    {
        var books = Context.Books
            .AsNoTracking()
            .Include(x => x.Category)
            .Include(x => x.BookAuthors)
            .ThenInclude(x => x.Author)
            .Include(x => x.Units)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Title))
            books = books.Where(x => x.Title.Contains(query.Title.Trim()));

        if (!string.IsNullOrWhiteSpace(query.Isbn))
            books = books.Where(x => x.Isbn.Contains(query.Isbn.Trim()));

        if (!string.IsNullOrWhiteSpace(query.Category))
            books = books.Where(x => x.Category != null && x.Category.Name.Contains(query.Category.Trim()));

        if (!string.IsNullOrWhiteSpace(query.Author))
            books = books.Where(x => x.BookAuthors.Any(a => a.Author != null && a.Author.Name.Contains(query.Author.Trim())));

        if (query.Available is true)
            books = books.Where(x => x.Units.Any(c => c.Status == BookUnitStatus.Available));

        if (query.Available is false)
            books = books.Where(x => !x.Units.Any(c => c.Status == BookUnitStatus.Available));

        var totalCount = await books.CountAsync(ct);
        var items = await books
            .OrderBy(x => x.Title)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<bool> IsbnExistsAsync(string isbn, Guid? excludedId = null, CancellationToken ct = default)
    {
        return await Context.Books.AnyAsync(x => x.Isbn == isbn && (excludedId == null || x.Id != excludedId), ct);
    }

    public async Task<Category?> GetCategoryByNameAsync(string name, CancellationToken ct = default)
    {
        return await Context.Categories.FirstOrDefaultAsync(x => x.Name == name, ct);
    }

    public async Task AddCategoryAsync(Category category, CancellationToken ct = default)
    {
        await Context.Categories.AddAsync(category, ct);
    }

    public async Task<IReadOnlyCollection<Author>> GetAuthorsByNamesAsync(IReadOnlyCollection<string> names, CancellationToken ct = default)
    {
        return await Context.Authors
            .Where(x => names.Contains(x.Name))
            .ToListAsync(ct);
    }

    public async Task AddAuthorAsync(Author author, CancellationToken ct = default)
    {
        await Context.Authors.AddAsync(author, ct);
    }

    public async Task ReplaceUnitsAsync(Book book, IReadOnlyCollection<BookUnit> units, CancellationToken ct = default)
    {
        Context.BookUnits.RemoveRange(book.Units);
        await Context.BookUnits.AddRangeAsync(units, ct);
        book.Units.Clear();

        foreach (var unit in units)
            book.Units.Add(unit);
    }
}
