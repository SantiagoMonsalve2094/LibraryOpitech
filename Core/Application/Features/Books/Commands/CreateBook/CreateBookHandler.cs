using FluentValidation;
using LibraryOpitech.Application.DTOs.Books;
using LibraryOpitech.Application.Features.Books.Queries.GetBooks;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Books.Commands.CreateBook;

public sealed class CreateBookHandler(IUnitOfWork uow, IValidator<CreateBookCommand> validator)
{
    public async Task<BookResponse> Handle(CreateBookCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        if (await uow.Books.IsbnExistsAsync(command.Isbn.Trim(), null, ct))
            throw new InvalidOperationException("A book with that ISBN already exists.");

        var category = await BookRules.GetOrCreateCategoryAsync(uow, command.CategoryName, ct);
        var authors = await BookRules.GetOrCreateAuthorsAsync(uow, command.AuthorNames, ct);

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = command.Title.Trim(),
            Isbn = command.Isbn.Trim(),
            PublicationYear = command.PublicationYear,
            Description = string.IsNullOrWhiteSpace(command.Description) ? null : command.Description.Trim(),
            Category = category,
            Units = BookRules.CreateUnits(command.Isbn.Trim(), command.Units, 0)
        };

        foreach (var author in authors)
        {
            book.BookAuthors.Add(new BookAuthor
            {
                BookId = book.Id,
                AuthorId = author.Id,
                Author = author
            });
        }

        await uow.Books.AddAsync(book, ct);
        await uow.SaveChangesAsync(ct);

        var created = await uow.Books.GetByIdWithDetailsAsync(book.Id, ct);
        return GetBooksHandler.ToResponse(created ?? book);
    }
}

internal static class BookRules
{
    internal static async Task<Category> GetOrCreateCategoryAsync(IUnitOfWork uow, string name, CancellationToken ct)
    {
        var cleanName = name.Trim();
        var category = await uow.Books.GetCategoryByNameAsync(cleanName, ct);

        if (category is not null)
            return category;

        category = new Category
        {
            Id = Guid.NewGuid(),
            Name = cleanName
        };

        await uow.Books.AddCategoryAsync(category, ct);
        return category;
    }

    internal static async Task<IReadOnlyCollection<Author>> GetOrCreateAuthorsAsync(IUnitOfWork uow, IReadOnlyCollection<string> names, CancellationToken ct)
    {
        var cleanNames = names.Select(x => x.Trim()).Where(x => x.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        var existingAuthors = await uow.Books.GetAuthorsByNamesAsync(cleanNames, ct);
        var authors = existingAuthors.ToList();

        foreach (var name in cleanNames)
        {
            if (authors.Any(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase)))
                continue;

            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = name
            };

            await uow.Books.AddAuthorAsync(author, ct);
            authors.Add(author);
        }

        return authors;
    }

    internal static List<BookUnit> CreateUnits(string isbn, int availableUnits, int unavailableUnits)
    {
        var units = new List<BookUnit>();

        for (var i = 1; i <= availableUnits; i++)
        {
            units.Add(new BookUnit
            {
                Id = Guid.NewGuid(),
                Code = $"{isbn}-A{i:000}",
                Status = BookUnitStatus.Available
            });
        }

        for (var i = 1; i <= unavailableUnits; i++)
        {
            units.Add(new BookUnit
            {
                Id = Guid.NewGuid(),
                Code = $"{isbn}-U{i:000}",
                Status = BookUnitStatus.Unavailable
            });
        }

        return units;
    }

}
