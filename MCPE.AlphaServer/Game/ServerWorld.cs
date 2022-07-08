using MCPE.AlphaServer.Network;
using MCPE.AlphaServer.RakNet;
using MCPE.AlphaServer.Utils;
using System.Collections.Generic;
using System.Numerics;

namespace MCPE.AlphaServer.Game;

public class ServerWorld {
    private GameServer Server;

    public List<Entity> Entities = new();
    private Dictionary<RakNetClient, ServerPlayer> ConnectionMap = new();
    public IEnumerable<ServerPlayer> Players => ConnectionMap.Values;

    public ServerWorld(GameServer server) => Server = server;

    // public RakNetConnection GetPlayerByName(string name) => Players.FirstOrDefault(P => P.Player?.Username == name);

    public ServerPlayer AddPlayer(RakNetClient client, ulong clientId, string username) {
        var newPlayer = new ServerPlayer(client) {
            PlayerID = clientId,
            Username = username
        };

        foreach (var player in Players) {
            player.Send(new AddPlayerPacket {
                PlayerId = newPlayer.PlayerID,
                Username = newPlayer.Username,
                EntityId = newPlayer.EntityID,
                Pos = newPlayer.Position,
            });
        }

        ConnectionMap.Add(client, newPlayer);
        return ConnectionMap[client];
    }

    public void RemovePlayer(RakNetClient client, string reason) {
        if (!ConnectionMap.TryGetValue(client, out var disconnectingPlayer))
            return;

        ConnectionMap.Remove(client);

        foreach (var player in Players) {
            player.Send(new RemovePlayerPacket {
                EntityId = disconnectingPlayer.EntityID,
                PlayerId = disconnectingPlayer.PlayerID,
            });

            player.Send(new MessagePacket {
                Username = "server",
                Message = $"{disconnectingPlayer.Username} left the game. ({reason})"
            });
        }

    }

    public void MovePlayer(RakNetClient client, Vector3 position, Vector3 viewAngle) {
        if (!ConnectionMap.TryGetValue(client, out var movingPlayer))
            return;

        movingPlayer.Position = position;
        movingPlayer.ViewAngle = viewAngle;

        foreach (var player in Players) {
            if (player.PlayerID == movingPlayer.PlayerID)
                continue;

            player.Send(new MovePlayerPacket {
                EntityId = movingPlayer.EntityID,
                Pos = movingPlayer.Position,
                Rot = movingPlayer.ViewAngle,
            });
        }
    }
}