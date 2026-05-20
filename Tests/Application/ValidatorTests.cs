using FluentAssertions;
using LibraryOpitech.Application.Features.Books.Commands.CreateBook;
using LibraryOpitech.Application.Features.Users.Commands.CreateUser;

namespace LibraryOpitech.Application.Tests;

public class ValidatorTests
{
    [Fact]
    public void CreateUserValidator_Should_Fail_When_Email_Is_Invalid()
    {
        var validator = new CreateUserValidator();
        var command = new CreateUserCommand
        {
            Name = "Test User",
            Email = "bad-email",
            Password = "123456",
            Role = "User"
        };

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CreateBookValidator_Should_Fail_When_Author_Is_Missing()
    {
        var validator = new CreateBookValidator();
        var command = new CreateBookCommand
        {
            Title = "Book",
            Isbn = "123",
            PublicationYear = 2024,
            CategoryName = "Fantasy",
            AuthorNames = [],
            Units = 1
        };

        var result = validator.Validate(command);

        result.IsValid.Should().BeFalse();
    }
}
