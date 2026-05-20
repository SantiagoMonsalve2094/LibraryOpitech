namespace LibraryOpitech.Application.Features.Books.Commands.UpdateBookInventory;

public sealed record UpdateBookInventoryCommand
{
    public int AvailableUnits { get; init; }
    public int UnavailableUnits { get; init; }
}
