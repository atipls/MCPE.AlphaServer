using MCPE.AlphaServer.RakNet;
using System.Numerics;

namespace MCPE.AlphaServer.Game;

public class Player : Entity {
    public RakNetClient Client;

    public ulong PlayerID;
    public string Username;
    public Vector3 Position = new(100.0f, 70.0f, 100.0f);
    public Vector3 ViewAngle = new();

    public const int SYNC_IS_SLEEPING = 16;
    public const int SYNC_SLEEP_POSITION = 17;

    public Player(RakNetClient client) {
        Client = client;

        Define(SYNC_IS_SLEEPING, EntityDataType.Byte);
        Define(SYNC_SLEEP_POSITION, EntityDataType.Pos);
    }

    public bool IsClientOf(RakNetClient client) => client.ClientID == Client.ClientID;
    public void Send(ConnectedPacket packet, int reliability = ConnectedPacket.RELIABLE) => Client.Send(packet, reliability);
}