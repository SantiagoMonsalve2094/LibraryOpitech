namespace LibraryOpitech.Application.DTOs.Reports;

public class MostBorrowedBookByCategoryResponse
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public int LoanCount { get; set; }
}
