namespace InventoryModule.Commands;

public record ReserveRoomStartedCommand(
    Guid saga_id,
    DateTime timestamp
    );