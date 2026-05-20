using LibraryOpitech.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryOpitech.Infrastructure.Persistence.Repositories;

public class Repository<T>(LibraryOpitechDbContext context) : IRepository<T> where T : class
{
    protected readonly LibraryOpitechDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync([id], ct);

    public async Task<IReadOnlyCollection<T>> GetAllAsync(CancellationToken ct = default)
        => await DbSet.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    public void Update(T entity)
        => Context.Update(entity);

    public void Delete(T entity)
        => DbSet.Remove(entity);
}
