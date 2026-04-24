namespace AuthModule.Commands;

public record AuthResponceCommand(
    string? jwt,
    bool success,
    string message
    );