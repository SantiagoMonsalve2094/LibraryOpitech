using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Domain.Entities;

public class BookUnit
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public BookUnitStatus Status { get; set; } = BookUnitStatus.Available;
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
    public ICollection<Loan> Loans { get; set; } = [];
}
