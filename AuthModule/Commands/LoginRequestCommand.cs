using MediatR;

namespace AuthModule.Commands;

public record LoginRequestCommand(
    string email,
    string password_plain
    ): IRequest<AuthResponceCommand>;