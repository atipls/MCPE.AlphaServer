using Serilog;

namespace MCPE.AlphaServer.Utils;

public static class Logger {
    public static ILogger LogBackend;

    public static void Debug(string message) => LogBackend?.Debug(message);
    public static void Info(string message) => LogBackend?.Information(message);
    public static void Warn(string message) => LogBackend?.Warning(message);
    public static void Error(string message) => LogBackend?.Error(message);
}