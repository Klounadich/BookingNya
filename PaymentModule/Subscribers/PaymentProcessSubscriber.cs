using System.Runtime.InteropServices.ComTypes;
using DotNetCore.CAP;
using PaymentModule.Commands;
using PaymentModule.Services;

namespace PaymentModule.Subscribers;

public class PaymentProcessSubscriber : ICapSubscribe
{
    private readonly ICapPublisher _capPublisher;
    private readonly IPaymentService _paymentService;

    public PaymentProcessSubscriber(IPaymentService paymentService , ICapPublisher capPublisher)
    {
        _paymentService = paymentService;
        _capPublisher = capPublisher;
    }
    [CapSubscribe("payment.process.payment.command")]
    public async Task HandleAsync(ProcessPaymentCommand command)
    {
        var result = await _paymentService.ProcessPaymentAsync(command);
        if (result.Status)
        {
            await _capPublisher.PublishAsync("payment.processed.event", new PaymentProcessed(command.SagaId ,result.Error));
        }
        else
        {
           Console.WriteLine(result.Error);
            await _capPublisher.PublishAsync("payment.failed.event", new PaymentProcessed(command.SagaId ,result.Error));
        }
    }
}