using FluentValidation;
using LibraryOpitech.Application.DTOs.Books;
using LibraryOpitech.Application.Features.Books.Commands.CreateBook;
using LibraryOpitech.Application.Features.Books.Queries.GetBooks;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookHandler(IUnitOfWork uow, IValidator<UpdateBookCommand> validator)
{
    public async Task<BookResponse?> Handle(Guid id, UpdateBookCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var book = await uow.Books.GetByIdWithDetailsAsync(id, ct);
        if (book is null) return null;

        var title = command.Title?.Trim() ?? book.Title;
        var isbn = command.Isbn?.Trim() ?? book.Isbn;
        var publicationYear = command.PublicationYear ?? book.PublicationYear;
        var categoryName = command.CategoryName?.Trim() ?? book.Category?.Name ?? string.Empty;
        var authorNames = command.AuthorNames ?? book.BookAuthors.Select(x => x.Author?.Name ?? string.Empty).ToList();

        if (await uow.Books.IsbnExistsAsync(isbn, id, ct))
            throw new InvalidOperationException("A book with that ISBN already exists.");

        book.Title = title;
        book.Isbn = isbn;
        book.PublicationYear = publicationYear;
        book.Description = command.Description is null ? book.Description : (string.IsNullOrWhiteSpace(command.Description) ? null : command.Description.Trim());

        if (command.CategoryName is not null)
            book.Category = await BookRules.GetOrCreateCategoryAsync(uow, command.CategoryName, ct);

        if (command.AuthorNames is not null)
        {
            var authors = await BookRules.GetOrCreateAuthorsAsync(uow, command.AuthorNames, ct);
            book.BookAuthors.Clear();

            foreach (var author in authors)
            {
                book.BookAuthors.Add(new BookAuthor
                {
                    BookId = book.Id,
                    AuthorId = author.Id,
                    Author = author
                });
            }
        }

        uow.Books.Update(book);
        await uow.SaveChangesAsync(ct);

        var updated = await uow.Books.GetByIdWithDetailsAsync(book.Id, ct);
        return GetBooksHandler.ToResponse(updated ?? book);
    }
}
