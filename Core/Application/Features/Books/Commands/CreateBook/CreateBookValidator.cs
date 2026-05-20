using FluentValidation;

namespace LibraryOpitech.Application.Features.Books.Commands.CreateBook;

public sealed class CreateBookValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Isbn)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(1000, DateTime.UtcNow.Year + 1);

        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.AuthorNames)
            .NotEmpty()
            .Must(authors => authors.Any(author => !string.IsNullOrWhiteSpace(author)))
            .WithMessage("At least one author is required.");

        RuleForEach(x => x.AuthorNames)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Units)
            .GreaterThanOrEqualTo(0);
    }
}
