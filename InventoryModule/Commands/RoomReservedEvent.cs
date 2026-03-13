namespace InventoryModule.Commands;

public record RoomReservedEvent(
    Guid SagaId,
    Guid ReservationId,
    string RoomId
);