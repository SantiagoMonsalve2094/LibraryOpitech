using FluentValidation;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Loans.Queries.GetLoans;

public sealed class GetLoansValidator : AbstractValidator<GetLoansQuery>
{
    public GetLoansValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
        RuleFor(x => x.Status)
            .Must(status => status is null || Enum.TryParse<LoanStatus>(status, true, out _))
            .WithMessage("Status must be Active or Returned.");
    }
}
