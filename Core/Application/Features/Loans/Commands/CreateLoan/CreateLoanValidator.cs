using FluentValidation;

namespace LibraryOpitech.Application.Features.Loans.Commands.CreateLoan;

public sealed class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.BookId).NotEmpty();
    }
}
