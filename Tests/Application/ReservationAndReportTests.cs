using FluentAssertions;
using LibraryOpitech.Application.DTOs.Reports;
using LibraryOpitech.Application.Features.Reports.Queries.GetMostBorrowedBooksByCategory;
using LibraryOpitech.Application.Features.Reservations.Commands.CreateReservation;
using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using Moq;

namespace LibraryOpitech.Application.Tests;

public class ReservationAndReportTests
{
    [Fact]
    public async Task CreateReservationHandler_Should_Reject_When_Book_Has_Available_Units()
    {
        var user = new User { Id = Guid.NewGuid(), IsActive = true };
        var book = new Book { Id = Guid.NewGuid(), Title = "Book" };

        var users = new Mock<IUserRepository>();
        users.Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var books = new Mock<IBookRepository>();
        books.Setup(x => x.GetByIdWithDetailsAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(book);

        var reservations = new Mock<IReservationRepository>();
        reservations.Setup(x => x.HasAvailableUnitsAsync(book.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Users).Returns(users.Object);
        uow.SetupGet(x => x.Books).Returns(books.Object);
        uow.SetupGet(x => x.Reservations).Returns(reservations.Object);

        var handler = new CreateReservationHandler(uow.Object, new CreateReservationValidator());
        var command = new CreateReservationCommand { UserId = user.Id, BookId = book.Id };

        var act = () => handler.Handle(command);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ReportHandler_Should_Return_Most_Borrowed_Books_By_Category()
    {
        var expected = new List<MostBorrowedBookByCategoryResponse>
        {
            new()
            {
                CategoryId = Guid.NewGuid(),
                CategoryName = "Sci-Fi",
                BookId = Guid.NewGuid(),
                BookTitle = "Dune",
                LoanCount = 3
            }
        };

        var reports = new Mock<IReportRepository>();
        reports.Setup(x => x.GetMostBorrowedBooksByCategoryAsync(It.IsAny<CancellationToken>())).ReturnsAsync(expected);

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Reports).Returns(reports.Object);

        var handler = new GetMostBorrowedBooksByCategoryHandler(uow.Object, new GetMostBorrowedBooksByCategoryValidator());
        var response = await handler.Handle(new GetMostBorrowedBooksByCategoryQuery());

        response.Should().ContainSingle();
        response.First().BookTitle.Should().Be("Dune");
    }
}
