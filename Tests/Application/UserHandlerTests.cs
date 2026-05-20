using FluentAssertions;
using LibraryOpitech.Application.Features.Users.Commands.CreateUser;
using LibraryOpitech.Application.Interfaces;
using Moq;

namespace LibraryOpitech.Application.Tests;

public class UserHandlerTests
{
    [Fact]
    public async Task CreateUserHandler_Should_Reject_Duplicate_Email()
    {
        var users = new Mock<IUserRepository>();
        users.Setup(x => x.EmailExistsAsync("test@library.com", null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Users).Returns(users.Object);

        var passwordHasher = new Mock<IPasswordHasher>();
        var handler = new CreateUserHandler(uow.Object, passwordHasher.Object, new CreateUserValidator());
        var command = new CreateUserCommand
        {
            Name = "Test User",
            Email = "test@library.com",
            Password = "123456",
            Role = "User"
        };

        var act = () => handler.Handle(command);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
