namespace AuthModule.Commands;

public record JWTRequestCommand(Guid id,
    string display_name,
    string email
    );