using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Domain.Entities;

public class Loan
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid BookUnitId { get; set; }
    public BookUnit? BookUnit { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public decimal FineAmount { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Active;
}
