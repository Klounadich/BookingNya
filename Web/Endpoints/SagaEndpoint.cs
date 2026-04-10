
using BookingModule.Commands;
using BookingNya.Validators;
using MediatR;
using FluentValidation;
using Shared.Enums;

namespace BookingNya.Endpoints;

public static class SagaEndpoint
{
    public static void MapSagaEndpont(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/").WithTags("Booking");
        group.MapPost("/booking", BookSaga);
        group.MapGet("/booking/{sagaId}", SagaStatus);
        group.MapPost("/booking/{sagaId}/confirm", ConfirmCode);
        group.MapPost("/booking/{sagaId}/retry", BookSagaRetry);
        group.MapPost("/booking/{sagaId}/compensate", BookSagaCompensate);
    }

    public async static Task<IResult> BookSaga(BookingRequestCommand data, IMediator mediator,
        IValidator<BookingRequestCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(data);
        if (validationResult.IsValid)
        {
            try
            {
                var start = await mediator.Send(data);

                if (start.Status == SagaTypes.Started || start.Status == SagaTypes.Running)
                {

                    return Results.Ok(new
                    {
                        Message = "Booking saga initiated.",
                        SagaId = start.SagaId,

                    });
                }

                return Results.BadRequest(new
                {
                    Error = "Failed to initiate booking saga.",
                    Details = start.Message
                });

            }
            catch (Exception)
            {

                return Results.Problem(statusCode: 500);
            }
        }

        

            var firsterror = validationResult.Errors.First().ErrorMessage;
            return Results.BadRequest(firsterror);
        
    }


public async static Task<IResult> ConfirmCode(ConfirmationCodeCommand data, IMediator mediator)
    {
        try
        {
            await mediator.Send(data);
            return Results.Accepted();
        }
        catch (Exception)
        {
            return Results.Problem( statusCode: 500);
        }
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