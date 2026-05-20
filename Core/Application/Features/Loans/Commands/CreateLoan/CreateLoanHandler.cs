using FluentValidation;
using LibraryOpitech.Application.DTOs.Loans;
using LibraryOpitech.Application.Features.Loans.Queries.GetLoans;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Application.Features.Loans.Commands.CreateLoan;

public sealed class CreateLoanHandler(IUnitOfWork uow, IValidator<CreateLoanCommand> validator)
{
    public async Task<LoanResponse> Handle(CreateLoanCommand command, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var user = await uow.Users.GetByIdAsync(command.UserId, ct)
            ?? throw new InvalidOperationException("User not found.");

        if (!user.IsActive)
            throw new InvalidOperationException("User is not active.");

        var book = await uow.Books.GetByIdWithDetailsAsync(command.BookId, ct)
            ?? throw new InvalidOperationException("Book not found.");

        var unit = await uow.Loans.GetFirstAvailableUnitAsync(command.BookId, ct)
            ?? throw new InvalidOperationException("Book has no available units.");

        unit.Status = BookUnitStatus.Loaned;

        var loan = new Loan
        {
            Id = Guid.NewGuid(),
            UserId = command.UserId,
            BookUnitId = unit.Id,
            LoanDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(30),
            FineAmount = 0,
            Status = LoanStatus.Active,
            User = user,
            BookUnit = unit
        };

        await uow.Loans.AddAsync(loan, ct);
        await uow.SaveChangesAsync(ct);

        loan.BookUnit.Book = book;
        return GetLoansHandler.ToResponse(loan);
    }
}
