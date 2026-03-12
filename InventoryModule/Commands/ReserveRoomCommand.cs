

namespace InventoryModule.Commands;

public record ReserveRoomCommand(
    Guid sagaId,
    string roomId,
    DateTime check_in,
    DateTime check_out

);

    
