
using BookingModule.Commands;
using BookingNya.Validators;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Shared.Enums;

namespace BookingNya.Endpoints;

public static class SagaEndpoint
{
    public static void MapSagaEndpont(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/").WithTags("Booking");
        group.MapPost("/booking", BookSaga);
        group.MapPost("/booking/{sagaId}/confirm", ConfirmCode);
        group.MapPost("/booking/{sagaId}/callback", SagaCallBack);
    }
    [Authorize]
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

[Authorize]
public async static Task<IResult> ConfirmCode(ConfirmationCodeCommand data, IMediator mediator)
    {
        try
        {
            await mediator.Send(data);
            return Results.Accepted();
        }
        catch (Exception ex)
        {
            
            return Results.Problem( statusCode: 500);
        }
    }


    [Authorize]
    public async static Task<IResult> SagaCallBack( CallBackSagaRequest request,IMediator mediator)
    {
        try
        {
            await mediator.Send(request);
            return Results.Accepted();
        }
        catch (Exception ex)
        {
            return Results.Problem( statusCode: 500);
        }
    }

}