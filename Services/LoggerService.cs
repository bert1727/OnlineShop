using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;

namespace OnlineShop.Services;

public static class LoggerService
{
    public static void SerilogLoggerService(
        this IServiceCollection services,
        ConfigurationManager config
    )
    {
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(config).CreateLogger();
        services.AddSerilog();
    }

    public static void SerilogLoggerServiceGlobal(this IHostBuilder host)
    {
        host.UseSerilog(
            (context, config) =>
            {
                config
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.File(
                        new CompactJsonFormatter(),
                        "./Logs/log1-.json",
                        rollingInterval: RollingInterval.Day
                    );
            }
        // new CompactJsonFormatter
        // new JsonFormatter(), */
        );
    }
}
