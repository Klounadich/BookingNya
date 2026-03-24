using DotNetCore.CAP;
using NotificationModule.Commands;
using NotificationModule.Services;

namespace NotificationModule.Subscribers;

public class ConfirmationSubscriber : ICapSubscribe
{
    private readonly ICapPublisher _capPublisher;
    private readonly INotificationService _notificationService;

    public ConfirmationSubscriber(INotificationService notificationService ,  ICapPublisher capPublisher)
    {
        _notificationService = notificationService;
        _capPublisher = capPublisher;
    }
    [CapSubscribe("notification.send.confirmation.command")]
    public async Task Handle(SendConfirmationCommand command)
    {
        if (await _notificationService.SendConfirmationAsync(command))
        {
            await _capPublisher.PublishAsync("notification.sent.event.success", new ConfirmationSentCommand(command.saga_id));
        }
        else
        {
            await _capPublisher.PublishAsync("notification.sent.event.failure", new ConfirmationSentCommand(command.saga_id));
        }
    }
}