using LibraryOpitech.Application.Interfaces;
using LibraryOpitech.Infrastructure.Persistence.Repositories;

namespace LibraryOpitech.Infrastructure.Persistence;

public class UnitOfWork(LibraryOpitechDbContext context) : IUnitOfWork
{
    private IUserRepository? users;
    private IBookRepository? books;
    private ILoanRepository? loans;
    private IReservationRepository? reservations;
    private IReportRepository? reports;

    public IUserRepository Users => users ??= new UserRepository(context);
    public IBookRepository Books => books ??= new BookRepository(context);
    public ILoanRepository Loans => loans ??= new LoanRepository(context);
    public IReservationRepository Reservations => reservations ??= new ReservationRepository(context);
    public IReportRepository Reports => reports ??= new ReportRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}
