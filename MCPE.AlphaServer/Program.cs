using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.RakNet;
using MCPE.AlphaServer.Utils;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace MCPE.AlphaServer;

internal static class Program {
    private static async Task Main(string[] _) {
#if DEBUG
        Directory.SetCurrentDirectory("/Users/atipls/work/MCPE.AlphaServer");
#endif

        var world = World.From("Data/MainWorld/");

        Logger.LogBackend = new LoggerConfiguration()
            .WriteTo.Console(theme: SystemConsoleTheme.Colored)
            .MinimumLevel.Debug()
            .CreateLogger();

        Logger.Info("MCPE.AlphaServer starting.");

        var server = new RakNetServer(19132) {
            ServerName = "MCPE.AlphaServer"
        };

        server.Start(new GameServer());

        Logger.Info("MCPE.AlphaServer started.");

        await Task.Delay(Timeout.Infinite);
    }
}
