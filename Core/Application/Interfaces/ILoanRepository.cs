using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Interfaces;

public interface ILoanRepository : IRepository<Loan>
{
    Task<Loan?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<BookUnit?> GetFirstAvailableUnitAsync(Guid bookId, CancellationToken ct = default);
    Task<(IReadOnlyCollection<Loan> Items, int TotalCount)> SearchAsync(Guid? userId, string? status, int page, int pageSize, CancellationToken ct = default);
}
