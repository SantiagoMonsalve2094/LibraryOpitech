using FluentValidation;
using LibraryOpitech.Application.Common;
using LibraryOpitech.Application.DTOs.Loans;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Features.Loans.Queries.GetLoans;

public sealed class GetLoansHandler(IUnitOfWork uow, IValidator<GetLoansQuery> validator)
{
    public async Task<PagedResult<LoanResponse>> Handle(GetLoansQuery query, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(query, ct);

        var (items, totalCount) = await uow.Loans.SearchAsync(query.UserId, query.Status, query.Page, query.PageSize, ct);

        return new PagedResult<LoanResponse>
        {
            Items = items.Select(ToResponse).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public static LoanResponse ToResponse(Loan loan)
    {
        return new LoanResponse
        {
            Id = loan.Id,
            UserId = loan.UserId,
            UserName = loan.User?.Name ?? string.Empty,
            BookId = loan.BookUnit?.BookId ?? Guid.Empty,
            BookTitle = loan.BookUnit?.Book?.Title ?? string.Empty,
            BookUnitId = loan.BookUnitId,
            BookUnitCode = loan.BookUnit?.Code ?? string.Empty,
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnedAt = loan.ReturnedAt,
            FineAmount = loan.FineAmount,
            Status = loan.Status.ToString()
        };
    }
}
