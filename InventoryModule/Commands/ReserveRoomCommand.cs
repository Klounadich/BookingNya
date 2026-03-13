

namespace InventoryModule.Commands;

public record ReserveRoomCommand(
    Guid sagaId,
    Guid bookingId,
    string roomId,
    DateTime check_in,
    DateTime check_out,
    string guest_name

);

    
