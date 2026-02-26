using System.Threading.Tasks.Dataflow;

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

    public static Task<IResult> BookSaga()
    {
        return Task.FromResult<IResult>(Results.Ok());
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