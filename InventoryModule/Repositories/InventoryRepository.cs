using System.Text.Json;
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
        var checkroom = await _context.Rooms
            .Where(x => x.id == roomId)
            .Select(x => x.status == RoomStatus.Available)
            .FirstOrDefaultAsync();
        return checkroom;
    }

    public async Task<bool> ReserveRoomAsync(RoomReservationModel data)
    {
        
       await _context.RoomReservations.AddAsync(data);
       var utcNow = DateTime.UtcNow;
       if (data.check_in == utcNow.Date)
       {
           var room = await _context.Rooms.FirstOrDefaultAsync(x => x.id == data.room_id);
           if (room != null)
           {
               room.status = RoomStatus.Occupied;
               _context.Rooms.Update(room);
               return await _context.SaveChangesAsync() > 0;
           }
            
       }

       if (await _context.SaveChangesAsync() > 0)
       {
           return true;
       }

       return false;

    }

       public async Task<FreeRoomsResponse> FreeRoomsAsync(RequestRoomFIltresCommand command)
    {
       
        
        var query = _context.Rooms.AsQueryable();

        bool isDateSearch = command.From.HasValue && command.To.HasValue;

        if (isDateSearch)
        {
            
        }
        else
        {
            query = query.Where(x => x.status == RoomStatus.Available);
        }

        if (isDateSearch)
        {
            var checkInUtc = DateTime.SpecifyKind(command.From.Value.Date, DateTimeKind.Utc);
            
            var checkOutUtc = DateTime.SpecifyKind(command.To.Value.Date, DateTimeKind.Utc);

            query = query.Where(room => 
                !_context.RoomReservations.Any(reservation => 
                    reservation.room_id == room.id &&
                    
                    reservation.check_in < checkOutUtc &&
                    reservation.check_out > checkInUtc
                )
            );
        }
        
        if (!string.IsNullOrEmpty(command.room_class))
            query = query.Where(x => x.type == command.room_class);

        if (command.capacity.HasValue)
            query = query.Where(x => x.capacity == command.capacity.Value);

        if (command.minimal_price.HasValue)
            query = query.Where(x => x.price_per_night >= command.minimal_price.Value);

        if (command.maximal_price.HasValue)
            query = query.Where(x => x.price_per_night <= command.maximal_price.Value);

        if (command.floor.HasValue)
            query = query.Where(x => x.floor == command.floor.Value);

        if (command.amenities != null && command.amenities.Any())
        {
            foreach (var amenity in command.amenities)
            {
                var key = amenity.Key;
                var value = amenity.Value;

                if (value is JsonElement jsonElement)
                {
                    if (jsonElement.ValueKind == JsonValueKind.True)
                    {
                        var jsonFilter = $"{{\"{key}\":true}}";
                        query = query.Where(x => EF.Functions.JsonContains(x.amenities, jsonFilter));
                    }
                    else if (jsonElement.ValueKind == JsonValueKind.False)
                    {
                        var jsonFilter = $"{{\"{key}\":true}}";
                        query = query.Where(x => !EF.Functions.JsonContains(x.amenities, jsonFilter));
                    }
                    else if (jsonElement.ValueKind == JsonValueKind.String)
                    {
                        var stringValue = jsonElement.GetString();
                       
                        var safeValue = JsonSerializer.Serialize(stringValue);
                        var jsonFilter = $"{{\"{key}\":{safeValue}}}";
                        query = query.Where(x => EF.Functions.JsonContains(x.amenities, jsonFilter));
                    }
                }
            }
        }

        var roomsFromDb = await query
            .Select(room => new {
                room.id,
                room.type,
                room.capacity,
                room.price_per_night,
                room.floor,
                room.description,
                room.amenities,
                Pictures = room.pictures 
            })
            .AsNoTracking()
            .ToListAsync();

        var responseRooms = roomsFromDb.Select(r => new FreeRoomsCommand(
            r.id,
            r.type,
            r.capacity,
            r.price_per_night,
            r.floor,
            r.description,
            r.amenities,
            photos: r.Pictures?.Select((bytes, index) => 
                $"http://localhost:5255/api/rooms/{r.id}/photos/{index}"
            ).ToList() ?? new List<string>()
        )).ToList();
        
        return new FreeRoomsResponse(
            Rooms: responseRooms,
            TotalCount: responseRooms.Count,
            Timestamp: DateTime.UtcNow,
            RequestId: command.requestId
        );
    }


    
    public async Task<bool> CallbackReserve(Guid sagaId)
    {
       var room_reservation = await _context.RoomReservations.Where(X => X.saga_id == sagaId).FirstOrDefaultAsync();
       if (room_reservation == null || room_reservation.status == ReservationStatus.Cancelled)
       {
           return false;
       }

       var status = await _context.Rooms.Where(x => x.id == room_reservation.room_id).FirstAsync();
       status.status = RoomStatus.Available;
       room_reservation.status = ReservationStatus.Cancelled;
       _context.RoomReservations.Update(room_reservation);
        _context.Rooms.Update(status);
        return await _context.SaveChangesAsync() > 0;
    }
}