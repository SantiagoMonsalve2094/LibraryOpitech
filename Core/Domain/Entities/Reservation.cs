using LibraryOpitech.Domain.Enums;

namespace LibraryOpitech.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
    public DateTime ReservedAt { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
}
