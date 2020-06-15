using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MCPE.AlphaServer {
    class Program {
        static async Task Main(string[] _) {
            Console.WriteLine("Starting MCPE.AlphaServer.");
            
            // For RakEncoder
            Debug.Assert(BitConverter.IsLittleEndian);

            Server server = new Server(19132);

            var listenerThread = new Thread(server.ListenerThread);
            var updaterThread = new Thread(server.ClientUpdaterThread);

            listenerThread.Name = "Listener Thread";
            updaterThread.Name = "Updater Thread";

            listenerThread.Start();
            updaterThread.Start();

            listenerThread.Join();
            updaterThread.Join();

            while (server.IsRunning)
                await Task.Delay(100);
        }
    }
}
