using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryOpitech.Infrastructure.Persistence.Repositories;

public class UserRepository(LibraryOpitechDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        var cleanEmail = email.Trim().ToLowerInvariant();

        return await Context.Users.FirstOrDefaultAsync(x => x.Email == cleanEmail, ct);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludedId = null, CancellationToken ct = default)
    {
        var cleanEmail = email.Trim().ToLowerInvariant();

        return await Context.Users.AnyAsync(x => x.Email == cleanEmail && (excludedId == null || x.Id != excludedId), ct);
    }

    public async Task<(IReadOnlyCollection<User> Items, int TotalCount)> SearchAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var users = Context.Users.AsNoTracking().OrderBy(x => x.Name);
        var totalCount = await users.CountAsync(ct);
        var items = await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return (items, totalCount);
    }
}
