using MediatR;

namespace AuthModule.Commands;

public record GetBalanceQuery(string userId) : IRequest<decimal>;