using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks.Dataflow;
using BookingModule.Commands;
using BookingModule.Models;
using BookingModule.Services;
using Shared.Enums;

namespace BookingNya.Endpoints;

public static class SagaEndpoint
{
    public static void MapSagaEndpont(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/").WithTags("Booking");
        group.MapPost("/booking" ,BookSaga);
        group.MapGet("/booking/{sagaId}", SagaStatus);
        group.MapPost("/booking/{sagaId}/retry" , BookSagaRetry);
        group.MapPost("/booking/{sagaId}/compensate", BookSagaCompensate);
    }

    public async static Task<IResult> BookSaga(BookingRequestCommand data , IBookingService service)
    {
      var result =  await service.StartBooking(data);
        return Results.Ok(result);
    }

    public static Task<IResult> SagaStatus(int sagaId)
    {
        return Task.FromResult<IResult>(Results.Ok());
    }
    
    
    public static Task<IResult> BookSagaRetry(int sagaId)
    {
        return Task.FromResult<IResult>(Results.Ok());
    }
    
    public static Task<IResult> BookSagaCompensate(int sagaId)
    {
        return Task.FromResult<IResult>(Results.Ok());
    }
}