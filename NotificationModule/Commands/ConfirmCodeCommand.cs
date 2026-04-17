namespace NotificationModule.Commands;

public record ConfirmCodeCommand
    (Guid SagaId, string ConfirmationCode , int attempts);
    
