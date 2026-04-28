namespace BookingModule.Commands;

public record GetBookingCard(
    string room_id,
    string guest_email,
    DateTime check_in,
    DateTime check_out,
    decimal total_price,
    string payment_method,
    Guid saga_id
    );