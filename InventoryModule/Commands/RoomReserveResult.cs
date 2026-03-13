namespace InventoryModule.Commands;

public record RoomReserveResult(
    Guid reservaiton_id,
    string Message
    );