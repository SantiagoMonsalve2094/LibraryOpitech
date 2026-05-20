namespace LibraryOpitech.Application.Features.Loans.Queries.GetLoans;

public sealed record GetLoansQuery
{
    public Guid? UserId { get; init; }
    public string? Status { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
