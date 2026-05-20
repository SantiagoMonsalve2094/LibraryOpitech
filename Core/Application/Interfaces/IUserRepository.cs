using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> EmailExistsAsync(string email, Guid? excludedId = null, CancellationToken ct = default);
    Task<(IReadOnlyCollection<User> Items, int TotalCount)> SearchAsync(int page, int pageSize, CancellationToken ct = default);
}
