using LibraryOpitech.Application.DTOs.Reports;

namespace LibraryOpitech.Application.Interfaces;

public interface IReportRepository
{
    Task<IReadOnlyCollection<MostBorrowedBookByCategoryResponse>> GetMostBorrowedBooksByCategoryAsync(CancellationToken ct = default);
}
