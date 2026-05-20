namespace LibraryOpitech.Application.Features.Loans.Commands.CreateLoan;

public sealed record CreateLoanCommand
{
    public Guid UserId { get; init; }
    public Guid BookId { get; init; }
}
