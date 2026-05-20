using FluentValidation;
using LibraryOpitech.Application.DTOs.Loans;
using LibraryOpitech.Application.Features.Loans.Queries.GetLoans;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Loans.Commands.ReturnLoan;

public sealed class ReturnLoanHandler(IUnitOfWork uow, IValidator<ReturnLoanCommand> validator)
{
    private const decimal FinePerDay = 500;

    public async Task<LoanResponse?> Handle(ReturnLoanCommand command, Guid currentUserId, bool isAdmin, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var loan = await uow.Loans.GetByIdWithDetailsAsync(command.LoanId, ct);
        if (loan is null) return null;

        if (!isAdmin && loan.UserId != currentUserId)
            throw new InvalidOperationException("You can only return your own loans.");

        if (loan.Status == LoanStatus.Returned)
            throw new InvalidOperationException("Loan is already returned.");

        var returnedAt = DateTime.UtcNow;
        var lateDays = Math.Max(0, (returnedAt.Date - loan.DueDate.Date).Days);

        loan.ReturnedAt = returnedAt;
        loan.FineAmount = lateDays * FinePerDay;
        loan.Status = LoanStatus.Returned;

        if (loan.BookUnit is not null)
            loan.BookUnit.Status = BookUnitStatus.Available;

        uow.Loans.Update(loan);
        await uow.SaveChangesAsync(ct);

        return GetLoansHandler.ToResponse(loan);
    }
}
