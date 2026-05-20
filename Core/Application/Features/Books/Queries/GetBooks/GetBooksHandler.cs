using FluentValidation;
using LibraryOpitech.Application.Common;
using LibraryOpitech.Application.DTOs.Books;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Books.Queries.GetBooks;

public sealed class GetBooksHandler(IUnitOfWork uow, IValidator<GetBooksQuery> validator)
{
    public async Task<PagedResult<BookResponse>> Handle(GetBooksQuery query, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(query, ct);

        var (items, totalCount) = await uow.Books.SearchAsync(query, ct);

        return new PagedResult<BookResponse>
        {
            Items = items.Select(ToResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public static BookResponse ToResponse(Book book)
    {
        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Isbn = book.Isbn,
            PublicationYear = book.PublicationYear,
            Description = book.Description,
            CategoryId = book.CategoryId,
            CategoryName = book.Category?.Name ?? string.Empty,
            TotalUnits = book.Units.Count,
            AvailableUnits = book.Units.Count(x => x.Status == BookUnitStatus.Available),
            Authors = book.BookAuthors
                .Where(x => x.Author is not null)
                .Select(x => new AuthorResponse { Id = x.AuthorId, Name = x.Author!.Name })
                .OrderBy(x => x.Name)
                .ToList()
        };
    }
}
