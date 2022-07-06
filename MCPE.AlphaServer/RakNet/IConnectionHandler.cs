using System;

namespace MCPE.AlphaServer.RakNet;

public interface IConnectionHandler {
    public void OnOpen(RakNetConnection address);
    public void OnClose(RakNetConnection address, string reason);
    public void OnData(RakNetConnection address, ReadOnlyMemory<byte> data);
}