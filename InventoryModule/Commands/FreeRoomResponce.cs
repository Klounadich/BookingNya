namespace InventoryModule.Commands;

public record FreeRoomsResponse(
    List<FreeRoomsCommand> Rooms,
    int TotalCount,
    DateTime Timestamp,
    Guid RequestId
);