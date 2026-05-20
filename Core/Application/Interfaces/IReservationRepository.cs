using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Interfaces;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<Reservation?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<bool> HasAvailableUnitsAsync(Guid bookId, CancellationToken ct = default);
    Task<(IReadOnlyCollection<Reservation> Items, int TotalCount)> SearchAsync(Guid? userId, string? status, int page, int pageSize, CancellationToken ct = default);
}
