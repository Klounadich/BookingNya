using MediatR;

namespace AuthModule.Commands;

public record RegisterRequestCommand(
    string email,
    string password,
    string dislay_name
    ) : IRequest<AuthResponceCommand>;