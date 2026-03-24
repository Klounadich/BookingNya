namespace NotificationModule.Commands;

public record SendConfirmationCommand(
    Guid saga_id,
    Guid booking_id,
    string email);