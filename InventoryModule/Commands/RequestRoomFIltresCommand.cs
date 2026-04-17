namespace InventoryModule.Commands;

public record RequestRoomFIltresCommand(
    Guid requestId ,
    DateTime? From,
    DateTime? To,
    string? room_class,
    int? capacity,
    decimal? minimal_price,
    decimal? maximal_price,
    int? floor,
    Dictionary<string,object>? amenities
    );