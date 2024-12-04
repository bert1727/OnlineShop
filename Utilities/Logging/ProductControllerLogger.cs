namespace OnlineShop.Utilities.Logging;

public static class ProductControllerLogger
{
    private static readonly Action<ILogger, string, Exception?> _logMessage =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(LogInfoProductController)),
            "Message from product controller: {Message}"
        );

    public static void LogInfoProductController(
        this ILogger logger,
        string message,
        Exception? exception = null
    )
    {
        _logMessage(logger, message, exception);
    }
}
