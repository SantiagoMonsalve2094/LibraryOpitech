using FluentValidation;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Books.Commands.DeleteBook;

public sealed class DeleteBookHandler(IUnitOfWork uow, IValidator<DeleteBookCommand> validator)
{
    public async Task<bool> Handle(DeleteBookCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var book = await uow.Books.GetByIdWithDetailsAsync(command.Id, ct);
        if (book is null) return false;

        uow.Books.Delete(book);
        await uow.SaveChangesAsync(ct);
        return true;
    }
}
