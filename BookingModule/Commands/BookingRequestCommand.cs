using MediatR;

namespace BookingModule.Commands;

public record BookingRequestCommand(
    Guid UserId,
string room_id ,
string guest_name,
string guest_email,
string guest_phone,
DateTime check_in ,
DateTime check_out,
decimal total_price,
string currency,
string payment_method,
string payment_reservation_id
    
    ): IRequest<StartSagaResult>;