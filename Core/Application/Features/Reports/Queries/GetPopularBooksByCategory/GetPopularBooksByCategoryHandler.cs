using FluentValidation;
using LibraryOpitech.Application.DTOs.Reports;
using LibraryOpitech.Application.Interfaces;

namespace LibraryOpitech.Application.Features.Reports.Queries.GetPopularBooksByCategory;

public sealed class GetPopularBooksByCategoryHandler(
    IUnitOfWork uow,
    IValidator<GetPopularBooksByCategoryQuery> validator)
{
    public async Task<IReadOnlyCollection<PopularBookByCategoryResponse>> Handle(
        GetPopularBooksByCategoryQuery query,
        CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(query, ct);
        return await uow.Reports.GetPopularBooksByCategoryAsync(ct);
    }
}
