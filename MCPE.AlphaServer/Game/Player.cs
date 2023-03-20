using MCPE.AlphaServer.RakNet;
using System.Numerics;

namespace MCPE.AlphaServer.Game;

public class Player : Entity {
    public RakNetClient Client;

    public ulong PlayerID;
    public string Username;
    public Vector3 Position = new(100.0f, 70.0f, 100.0f);
    public Vector3 ViewAngle = new();

    public Player(RakNetClient client) {
        Client = client;

        Define(EntityDataKey.IsSleeping, EntityDataType.Byte);
        Define(EntityDataKey.SleepPosition, EntityDataType.Pos);
    }

    public bool IsClientOf(RakNetClient client) => client.ClientID == Client.ClientID;

    public void Send(ConnectedPacket packet, int reliability = ConnectedPacket.RELIABLE) =>
        Client.Send(packet, reliability);
}
