using FluentValidation;

namespace LibraryOpitech.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
