using FluentValidation;
using LibraryOpitech.Application.DTOs.Reports;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Reports.Queries.GetMostBorrowedBooksByCategory;

public sealed class GetMostBorrowedBooksByCategoryHandler(
    IUnitOfWork uow,
    IValidator<GetMostBorrowedBooksByCategoryQuery> validator)
{
    public async Task<IReadOnlyCollection<MostBorrowedBookByCategoryResponse>> Handle(
        GetMostBorrowedBooksByCategoryQuery query,
        CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(query, ct);
        return await uow.Reports.GetMostBorrowedBooksByCategoryAsync(ct);
    }
}
