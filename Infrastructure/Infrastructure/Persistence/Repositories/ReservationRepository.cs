using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using LibraryOpitech.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibraryOpitech.Infrastructure.Persistence.Repositories;

public class ReservationRepository(LibraryOpitechDbContext context) : Repository<Reservation>(context), IReservationRepository
{
    public async Task<Reservation?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await Context.Reservations
            .Include(x => x.User)
            .Include(x => x.Book)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<bool> HasAvailableUnitsAsync(Guid bookId, CancellationToken ct = default)
    {
        return await Context.BookUnits.AnyAsync(x => x.BookId == bookId && x.Status == BookUnitStatus.Available, ct);
    }

    public async Task<(IReadOnlyCollection<Reservation> Items, int TotalCount)> SearchAsync(
        Guid? userId,
        string? status,
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var reservations = Context.Reservations
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.Book)
            .AsQueryable();

        if (userId is not null)
            reservations = reservations.Where(x => x.UserId == userId);

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<ReservationStatus>(status, true, out var parsedStatus))
            reservations = reservations.Where(x => x.Status == parsedStatus);

        var totalCount = await reservations.CountAsync(ct);
        var items = await reservations
            .OrderByDescending(x => x.ReservedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
