using FluentValidation;

namespace LibraryOpitech.Application.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200)
            .When(x => x.Title is not null);

        RuleFor(x => x.Isbn)
            .NotEmpty()
            .MaximumLength(30)
            .When(x => x.Isbn is not null);

        RuleFor(x => x.PublicationYear)
            .InclusiveBetween(1000, DateTime.UtcNow.Year + 1)
            .When(x => x.PublicationYear is not null);

        RuleFor(x => x.CategoryName)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => x.CategoryName is not null);

        RuleFor(x => x.AuthorNames)
            .Must(authors => authors is null || authors.Any(author => !string.IsNullOrWhiteSpace(author)))
            .WithMessage("At least one author is required.");

        RuleForEach(x => x.AuthorNames)
            .NotEmpty()
            .MaximumLength(150);
    }
}
