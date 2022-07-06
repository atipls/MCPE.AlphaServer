using System;
using MCPE.AlphaServer.RakNet;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer;

public class GameServer : IConnectionHandler {
    public void OnOpen(RakNetConnection address) {
        Logger.Debug($"[+] {address}");
        throw new NotImplementedException();
    }

    public void OnClose(RakNetConnection address, string reason) {
        Logger.Debug($"[-] {address} ({reason})");
    }

    public void OnData(RakNetConnection address, ReadOnlyMemory<byte> data) {
        Logger.Debug($"Got data from {address}, {data.Length} bytes");
    }
}