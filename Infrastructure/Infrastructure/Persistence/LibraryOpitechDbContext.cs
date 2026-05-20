using LibraryOpitech.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryOpitech.Infrastructure.Persistence;

public class LibraryOpitechDbContext(DbContextOptions<LibraryOpitechDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
    public DbSet<BookUnit> BookUnits => Set<BookUnit>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LibraryOpitechDbContext).Assembly);
    }
}
