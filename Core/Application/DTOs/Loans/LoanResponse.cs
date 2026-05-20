namespace LibraryOpitech.Application.DTOs.Loans;

public class LoanResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public Guid BookUnitId { get; set; }
    public string BookUnitCode { get; set; } = string.Empty;
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedAt { get; set; }
    public decimal FineAmount { get; set; }
    public string Status { get; set; } = string.Empty;
}
