using FluentAssertions;
using LibraryOpitech.Application.Features.Loans.Commands.CreateLoan;
using LibraryOpitech.Application.Features.Loans.Commands.ReturnLoan;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;
using Moq;

namespace LibraryOpitech.Application.Tests;

public class LoanHandlerTests
{
    [Fact]
    public async Task CreateLoanHandler_Should_Take_Available_Unit_And_Mark_It_As_Loaned()
    {
        var user = new User { Id = Guid.NewGuid(), Name = "Reader", IsActive = true };
        var book = new Book { Id = Guid.NewGuid(), Title = "Dune" };
        var unit = new BookUnit { Id = Guid.NewGuid(), BookId = book.Id, Book = book, Code = "DUNE-A001", Status = BookUnitStatus.Available };
        Loan? savedLoan = null;

        var users = new Mock<IUserRepository>();
        users.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var books = new Mock<IBookRepository>();
        books.Setup(x => x.GetByIdWithDetailsAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);

        var loans = new Mock<ILoanRepository>();
        loans.Setup(x => x.GetFirstAvailableUnitAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(unit);
        loans.Setup(x => x.AddAsync(It.IsAny<Loan>(), It.IsAny<CancellationToken>()))
            .Callback<Loan, CancellationToken>((loan, _) => savedLoan = loan)
            .Returns(Task.CompletedTask);

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Users).Returns(users.Object);
        uow.SetupGet(x => x.Books).Returns(books.Object);
        uow.SetupGet(x => x.Loans).Returns(loans.Object);

        var handler = new CreateLoanHandler(uow.Object, new CreateLoanValidator());
        var response = await handler.Handle(new CreateLoanCommand { UserId = user.Id, BookId = book.Id });

        unit.Status.Should().Be(BookUnitStatus.Loaned);
        savedLoan.Should().NotBeNull();
        response.BookUnitId.Should().Be(unit.Id);
    }

    [Fact]
    public async Task ReturnLoanHandler_Should_Calculate_Fine_After_Thirty_Days()
    {
        var userId = Guid.NewGuid();
        var loan = new Loan
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LoanDate = DateTime.UtcNow.AddDays(-40),
            DueDate = DateTime.UtcNow.AddDays(-10),
            Status = LoanStatus.Active,
            BookUnit = new BookUnit
            {
                Id = Guid.NewGuid(),
                Code = "UNIT-001",
                Status = BookUnitStatus.Loaned,
                Book = new Book { Id = Guid.NewGuid(), Title = "Book" }
            }
        };

        var loans = new Mock<ILoanRepository>();
        loans.Setup(x => x.GetByIdWithDetailsAsync(loan.Id, It.IsAny<CancellationToken>())).ReturnsAsync(loan);

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Loans).Returns(loans.Object);

        var handler = new ReturnLoanHandler(uow.Object, new ReturnLoanValidator());
        var response = await handler.Handle(new ReturnLoanCommand(loan.Id), userId, false);

        response.Should().NotBeNull();
        response!.FineAmount.Should().Be(5000);
        loan.BookUnit.Status.Should().Be(BookUnitStatus.Available);
        loan.Status.Should().Be(LoanStatus.Returned);
    }
}
