using FluentValidation;

namespace LibraryOpitech.Application.Features.Books.Queries.GetBooks;

public sealed class GetBooksValidator : AbstractValidator<GetBooksQuery>
{
    public GetBooksValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}
