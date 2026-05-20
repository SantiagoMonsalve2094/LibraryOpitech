using FluentValidation;
using LibraryOpitech.Application.DTOs.Books;
using LibraryOpitech.Application.Features.Books.Queries.GetBooks;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Books.Queries.GetBookById;

public sealed class GetBookByIdHandler(IUnitOfWork uow, IValidator<GetBookByIdQuery> validator)
{
    public async Task<BookResponse?> Handle(GetBookByIdQuery query, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(query, ct);

        var book = await uow.Books.GetByIdWithDetailsAsync(query.Id, ct);
        return book is null ? null : GetBooksHandler.ToResponse(book);
    }
}
