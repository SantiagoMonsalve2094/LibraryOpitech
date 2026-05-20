namespace LibraryOpitech.Application.DTOs.Reservations;

public class ReservationResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public DateTime ReservedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
