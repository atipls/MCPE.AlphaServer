using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MCPE.AlphaServer.Network;
using MCPE.AlphaServer.RakNet;

namespace MCPE.AlphaServer.Game;

public class ServerWorld {
    private readonly Dictionary<RakNetClient, Player> ConnectionMap = new();

    public List<Entity> Entities = new();
    private GameServer Server;

    public ServerWorld(GameServer server) {
        Server = server;
    }

    public IEnumerable<Player> Players => ConnectionMap.Values;

    public void SendAll(ConnectedPacket packet, ulong except = 0) {
        foreach (var player in Players)
            if (player.PlayerID != except)
                player.Send(packet);
    }

    public Player GetByName(string name) {
        return Players.FirstOrDefault(x => x.Username.ToLower() == name.ToLower(), null);
    }

    // public RakNetConnection GetPlayerByName(string name) => Players.FirstOrDefault(P => P.Player?.Username == name);

    public Player AddPlayer(RakNetClient client, ulong clientId, string username) {
        var newPlayer = new Player(client) {
            PlayerID = clientId,
            Username = username
        };

        SendAll(new AddPlayerPacket {
                PlayerId = newPlayer.PlayerID,
                Username = newPlayer.Username,
                EntityId = newPlayer.EntityID,
                Pos = newPlayer.Position
            }
        );


        ConnectionMap.Add(client, newPlayer);
        return ConnectionMap[client];
    }

    public void RemovePlayer(RakNetClient client, string reason) {
        if (!ConnectionMap.TryGetValue(client, out var disconnectingPlayer))
            return;

        ConnectionMap.Remove(client);

        SendAll(new RemovePlayerPacket {
                EntityId = disconnectingPlayer.EntityID,
                PlayerId = disconnectingPlayer.PlayerID
            }
        );

        SendAll(new ChatPacket {
                Message = $"{disconnectingPlayer.Username} left the game. ({reason})"
            }
        );
    }

    public void MovePlayer(RakNetClient client, Vector3 position, Vector3 viewAngle) {
        if (!ConnectionMap.TryGetValue(client, out var movingPlayer))
            return;

        movingPlayer.Position = position;
        movingPlayer.ViewAngle = viewAngle;

        SendAll(new MovePlayerPacket {
                EntityId = movingPlayer.EntityID,
                Pos = movingPlayer.Position,
                Rot = movingPlayer.ViewAngle
            }, movingPlayer.PlayerID
        );
    }
}
