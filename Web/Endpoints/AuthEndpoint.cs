using AuthModule.Commands;
using FluentValidation;
using MediatR;

namespace BookingNya.Endpoints;

public static class AuthEndpoint
{
    public static void MapAuthEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("api/").WithTags("Auth");
        group.MapPost("/auth/reg" ,RegistrationAsync);
        group.MapPost("/auth/login" ,LoginAsync);
        
    }

    public static async Task<IResult> RegistrationAsync(RegisterRequestCommand request , IMediator mediator , HttpContext ctx ,  IValidator<RegisterRequestCommand> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (validationResult.IsValid)
        {
            
       var result = await mediator.Send(request);
       if (result.success)
       {
           ctx.Response.Cookies.Append("auth_token", result.jwt, new CookieOptions
           {
               HttpOnly = true,
               SameSite = SameSiteMode.Lax,
               Secure = false,
               Expires = DateTime.Now.AddDays(1)
           });
           return Results.Ok(result);
       }
           return Results.BadRequest(result);
        }
        var firsterror = validationResult.Errors.First().ErrorMessage;
        return Results.BadRequest(firsterror);
    }

    public static async Task<IResult> LoginAsync(LoginRequestCommand request, IMediator mediator, HttpContext ctx)
    {
        
        var result = await mediator.Send(request);
        if (result.success)
        {
            ctx.Response.Cookies.Append("auth_token", result.jwt, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = false,
                Expires = DateTime.Now.AddDays(1)
            });
            return Results.Ok(result);
        }
        else
        {
            return Results.BadRequest(result);
        }
    }
}