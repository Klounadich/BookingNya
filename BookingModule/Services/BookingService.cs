using BookingModule.Commands;
using BookingModule.Models;
using BookingModule.Repositories;
using InventoryModule.Commands;
using Shared.Enums;
using DotNetCore.CAP;
using NotificationModule.Commands;

namespace BookingModule.Services;

public class BookingService : IBookingService
{
    private readonly ICapPublisher _capPublisher;   
    private readonly IBookingRepository _bookingRepository;
   
    

    public BookingService(IBookingRepository bookingRepository , ICapPublisher capPublisher)
    {
        _bookingRepository = bookingRepository;
        _capPublisher = capPublisher;
    }
    public async Task<StartSagaResult> StartBooking(BookingRequestCommand data)
    {
        var sagaId = Guid.NewGuid();    
        var bookingId = Guid.NewGuid(); 

       
        var booking = new BookingModel
        {
            id = bookingId,             
            saga_id = sagaId,            
            room_id = data.room_id,
            guest_name = data.guest_name,
            guest_phone = data.guest_phone,
            guest_email = data.guest_email,
            check_in = data.check_in,
            check_out = data.check_out,
            total_price = data.total_price,
            status = BookingStatus.Pending,
            created_at = DateTime.UtcNow,
            payment_method = data.payment_method,
            currency = data.currency,
            updated_at = DateTime.UtcNow,
            payment_reservation_id = data.payment_reservation_id,
            
            
        };

       
        var sagaState = new SagaStatesModel
        {
            saga_id = sagaId,            
            status = SagaTypes.Started,
            current_step = "ReserveRoom",
            started_at = DateTime.UtcNow,
            last_updated_at = DateTime.UtcNow
        };


        if (await _bookingRepository.StartSaga(sagaState, booking) == true)
        {
            await _capPublisher.PublishAsync("inventory.reserve.room.command", new ReserveRoomCommand(
                sagaId,
                bookingId,
                data.room_id,
                data.check_in,
                data.check_out,
                data.guest_name
            ));
            return new StartSagaResult(sagaId, SagaTypes.Started , "Saga Started");
        }
        return new StartSagaResult(sagaId, SagaTypes.Failed , "Failed Starting Saga");
    }

    public async Task ConfirmCode(ConfirmationCodeCommand data)
    {
        await _capPublisher.PublishAsync("notification.confirm.event", new ConfirmCodeCommand(
                data.SagaId,
                data.Code)
        );
    }
    
    }
    
