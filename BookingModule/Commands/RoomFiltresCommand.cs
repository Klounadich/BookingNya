using InventoryModule.Commands;
using MediatR;

namespace BookingModule.Commands;

public record RoomFiltresCommand
(
    
    DateTime? From,
    DateTime? To,
    string? room_class,
    int? capacity,
    decimal? minimal_price,
    decimal? maximal_price,
    int? floor,
    Dictionary<string,object>? amenities
    
    ) : IRequest<FreeRoomsResponse>;