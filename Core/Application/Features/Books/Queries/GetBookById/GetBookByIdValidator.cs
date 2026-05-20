using FluentValidation;

namespace LibraryOpitech.Application.Features.Books.Queries.GetBookById;

public sealed class GetBookByIdValidator : AbstractValidator<GetBookByIdQuery>
{
    public GetBookByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
