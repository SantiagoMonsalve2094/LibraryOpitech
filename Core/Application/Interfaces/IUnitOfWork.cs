namespace LibraryOpitech.Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IBookRepository Books { get; }
    ILoanRepository Loans { get; }
    IReservationRepository Reservations { get; }
    IReportRepository Reports { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
