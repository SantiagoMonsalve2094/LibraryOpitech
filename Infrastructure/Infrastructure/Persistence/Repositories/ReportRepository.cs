using LibraryOpitech.Application.DTOs.Reports;
using LibraryOpitech.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryOpitech.Infrastructure.Persistence.Repositories;

public class ReportRepository(LibraryOpitechDbContext context) : IReportRepository
{
    public async Task<IReadOnlyCollection<MostBorrowedBookByCategoryResponse>> GetMostBorrowedBooksByCategoryAsync(CancellationToken ct = default)
    {
        var groupedLoans = await context.Loans
            .AsNoTracking()
            .Where(x => x.BookUnit != null && x.BookUnit.Book != null && x.BookUnit.Book.Category != null)
            .Select(x => new
            {
                CategoryId = x.BookUnit!.Book!.CategoryId,
                CategoryName = x.BookUnit.Book.Category!.Name,
                BookId = x.BookUnit.BookId,
                BookTitle = x.BookUnit.Book.Title
            })
            .GroupBy(x => new
            {
                x.CategoryId,
                x.CategoryName,
                x.BookId,
                x.BookTitle
            })
            .Select(x => new MostBorrowedBookByCategoryResponse
            {
                CategoryId = x.Key.CategoryId,
                CategoryName = x.Key.CategoryName,
                BookId = x.Key.BookId,
                BookTitle = x.Key.BookTitle,
                LoanCount = x.Count()
            })
            .ToListAsync(ct);

        return groupedLoans
            .GroupBy(x => new { x.CategoryId, x.CategoryName })
            .Select(x => x.OrderByDescending(book => book.LoanCount).ThenBy(book => book.BookTitle).First())
            .OrderBy(x => x.CategoryName)
            .ToList();
    }
}
