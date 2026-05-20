using FluentValidation;
using LibraryOpitech.Application.DTOs.Books;
using LibraryOpitech.Application.Features.Books.Commands.CreateBook;
using LibraryOpitech.Application.Features.Books.Queries.GetBooks;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Books.Commands.UpdateBookInventory;

public sealed class UpdateBookInventoryHandler(IUnitOfWork uow, IValidator<UpdateBookInventoryCommand> validator)
{
    public async Task<BookResponse?> Handle(Guid id, UpdateBookInventoryCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var book = await uow.Books.GetByIdWithDetailsAsync(id, ct);
        if (book is null) return null;

        await uow.Books.ReplaceUnitsAsync(
            book,
            BookRules.CreateUnits(book.Isbn, command.AvailableUnits, command.UnavailableUnits),
            ct);

        await uow.SaveChangesAsync(ct);

        var updated = await uow.Books.GetByIdWithDetailsAsync(book.Id, ct);
        return GetBooksHandler.ToResponse(updated ?? book);
    }
}
