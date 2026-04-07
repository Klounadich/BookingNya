namespace InventoryModule.Commands;

public record FreeRoomsCommand(
    string RoomId,
    string type,
    int capacity,
    decimal price_per_night,
    int? floor,
    string description,
    Dictionary<string,object>? amenities);