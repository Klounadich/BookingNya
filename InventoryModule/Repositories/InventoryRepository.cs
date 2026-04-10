using InventoryModule.Commands;
using InventoryModule.Infrastructure;
using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace InventoryModule.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsRoomAvailableAsync(string roomId)
    {

        var checkroom = await _context.Rooms.Where(x => x.id == roomId).Select((x => x.status == RoomStatus.Available))
            .FirstOrDefaultAsync();
        return checkroom;
    }

    public async Task<bool> ReserveRoomAsync(RoomReservationModel data)
    {
        _context.RoomReservations.Add(data);
        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }

        return false;
    }

    public async Task<FreeRoomsResponse> FreeRoomsAsync(RequestRoomFIltresCommand command)
    {

        var query = _context.Rooms.Where(x => x.status == RoomStatus.Available);


        if (!string.IsNullOrEmpty(command.room_class))
        {
            query = query.Where(x => x.type == command.room_class);
        }

        if (command.capacity.HasValue)
        {
            var capacity = command.capacity.Value;
            query = query.Where(x => x.capacity == capacity);
        }

        if (command.minimal_price.HasValue)
        {
            var minPrice = command.minimal_price.Value;
            query = query.Where(x => x.price_per_night >= minPrice);
        }

        if (command.maximal_price.HasValue)
        {
            var maxPrice = command.maximal_price.Value;
            query = query.Where(x => x.price_per_night <= maxPrice);
        }

        if (command.floor.HasValue)
        {
            var floor = command.floor.Value;
            query = query.Where(x => x.floor == floor);
        }

        if (command.amenities != null && command.amenities.Any())
        {
            foreach (var amenity in command.amenities)
            {
                var key = amenity.Key;
                var value = amenity.Value;

                if (value is bool boolValue)
                {
                    var searchString = $"\"{key}\":{boolValue.ToString().ToLower()}";
                    query = query.Where(x => 
                        EF.Property<string>(x, "amenities") != null &&
                        EF.Property<string>(x, "amenities").ToString().Contains(searchString));
                }
                else if (value is string stringValue)
                {
                    var searchString = $"\"{key}\":\"{stringValue}\"";
                    query = query.Where(x => 
                        EF.Property<string>(x, "amenities") != null &&
                        EF.Property<string>(x, "amenities").ToString().Contains(searchString));
                }
            }
        }

        var availableRooms = await query
            .Select(room => new FreeRoomsCommand(
                room.id,
                room.type,
                room.capacity,
                room.price_per_night,
                room.floor,
                room.description,
                room.amenities
            ))
            .AsNoTracking()
            .ToListAsync();

        return new FreeRoomsResponse(
            availableRooms,
            availableRooms.Count,
            DateTime.UtcNow,
            command.requestId
        );
    }
}