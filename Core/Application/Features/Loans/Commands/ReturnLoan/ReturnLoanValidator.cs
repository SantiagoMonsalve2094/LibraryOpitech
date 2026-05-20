using FluentValidation;

namespace LibraryOpitech.Application.Features.Loans.Commands.ReturnLoan;

public sealed class ReturnLoanValidator : AbstractValidator<ReturnLoanCommand>
{
    public ReturnLoanValidator()
    {
        RuleFor(x => x.LoanId).NotEmpty();
    }
}
