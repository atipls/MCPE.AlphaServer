using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MCPE.AlphaServer.Data;
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

        var worldManager = new WorldManager("Data/MainWorld/");

        var chunk = worldManager.Chunks[0, 0];

        for (var y = 0; y < 128; y++) {
            for (var z = 0; z < 16; z++) {
                for (var x = 0; x < 16; x++) {
                    var block = chunk[x, y, z].ID;
                    Console.Write(block == 0 ? " " : "X");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }


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