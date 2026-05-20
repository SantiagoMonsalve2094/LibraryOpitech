using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryOpitech.Infrastructure.Persistence.Repositories;

public class LoanRepository(LibraryOpitechDbContext context) : Repository<Loan>(context), ILoanRepository
{
    public async Task<Loan?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await Context.Loans
            .Include(x => x.User)
            .Include(x => x.BookUnit)
            .ThenInclude(x => x!.Book)
            .ThenInclude(x => x!.Category)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<BookUnit?> GetFirstAvailableUnitAsync(Guid bookId, CancellationToken ct = default)
    {
        return await Context.BookUnits
            .Where(x => x.BookId == bookId && x.Status == BookUnitStatus.Available)
            .OrderBy(x => x.Code)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<(IReadOnlyCollection<Loan> Items, int TotalCount)> SearchAsync(
        Guid? userId,
        string? status,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var loans = Context.Loans
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.BookUnit)
            .ThenInclude(x => x!.Book)
            .AsQueryable();

        if (userId is not null)
            loans = loans.Where(x => x.UserId == userId);

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<LoanStatus>(status, true, out var parsedStatus))
            loans = loans.Where(x => x.Status == parsedStatus);

        var totalCount = await loans.CountAsync(ct);
        var items = await loans
            .OrderByDescending(x => x.LoanDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
