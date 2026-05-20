using FluentAssertions;
using LibraryOpitech.Application.Features.Books.Commands.CreateBook;
using LibraryOpitech.Application.Interfaces;
using Moq;

namespace LibraryOpitech.Application.Tests;

public class BookHandlerTests
{
    [Fact]
    public async Task CreateBookHandler_Should_Reject_Duplicate_Isbn()
    {
        var books = new Mock<IBookRepository>();
        books.Setup(x => x.IsbnExistsAsync("123", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Books).Returns(books.Object);

        var handler = new CreateBookHandler(uow.Object, new CreateBookValidator());
        var command = new CreateBookCommand
        {
            Title = "Book",
            Isbn = "123",
            PublicationYear = 2024,
            CategoryName = "Fantasy",
            AuthorNames = ["Author"],
            Units = 1
        };

        var act = () => handler.Handle(command);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
