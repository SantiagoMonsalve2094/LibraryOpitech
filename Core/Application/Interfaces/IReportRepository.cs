using LibraryOpitech.Application.DTOs.Reports;

namespace LibraryOpitech.Application.Interfaces;

public interface IReportRepository
{
    Task<IReadOnlyCollection<PopularBookByCategoryResponse>> GetPopularBooksByCategoryAsync(CancellationToken ct = default);
}
