using FluentValidation;

namespace LibraryOpitech.Application.Features.Books.Commands.DeleteBook;

public sealed class DeleteBookValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
