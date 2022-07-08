using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Network;
using MCPE.AlphaServer.RakNet;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer;

public class GameServer : IConnectionHandler {
    const int PROTOCOL = 14;
    private readonly List<string> BadUsernames = new() {
        "server",
        "rcon",
        "console",
    };

    private readonly ServerWorld World;

    public GameServer() {
        World = new ServerWorld(this);
    }

    public void OnOpen(RakNetClient client) {
        Logger.Debug($"[+] {client}");
    }

    public void OnClose(RakNetClient client, string reason) {
        Logger.Debug($"[-] {client} ({reason})");

        World.RemovePlayer(client, reason);
    }

    public void OnData(RakNetClient client, ReadOnlyMemory<byte> data) {
        var packet = MinecraftPacket.Parse(data);
        var packetName = packet.GetType().Name[0..^6];
        var handlerMethod = GetType().GetMethod($"Handle{packetName}", BindingFlags.Public | BindingFlags.Instance);

        if (handlerMethod == null)
            Logger.Warn($"Handler not implemented for {packetName}. Bug?");

        handlerMethod?.Invoke(this, new object[] { client, packet });
    }

    public virtual void HandleLoginRequest(RakNetClient client, LoginRequestPacket packet) {
        var responseStatus = LoginResponsePacket.StatusFor(packet.Protocol1, packet.Protocol2, PROTOCOL);
        var shouldRejectLogin = BadUsernames.Contains(packet.Username.ToLower())
            || false /* If it's already logged in. */;

        if (shouldRejectLogin) responseStatus = LoginResponsePacket.LoginStatus.ClientOutdated;

        client.Send(new LoginResponsePacket { Status = responseStatus });

        if (responseStatus != LoginResponsePacket.LoginStatus.VersionsMatch)
            return;

        // We can log in, start the game.

        var newPlayer = World.AddPlayer(client, packet.ClientId, packet.Username);
        client.Send(new StartGamePacket {
            Seed = 1,
            Pos = newPlayer.Position,
            EntityId = newPlayer.EntityID,
        });
    }

    public virtual void HandleReady(RakNetClient client, ReadyPacket packet) {
        // Notify this new player about other existing players.

        foreach (var player in World.Players) {
            if (player.IsClientOf(client))
                continue;

            client.Send(new AddPlayerPacket {
                PlayerId = player.PlayerID,
                Username = player.Username,
                EntityId = player.EntityID,
                Pos = player.Position,
                Pitch = (byte)(player.ViewAngle.Y / 360.0f * 256.0f),
                Yaw = (byte)(player.ViewAngle.X / 360.0f * 256.0f),
            });
        }

        // TODO: SetTime, maybe other things?
    }

    public virtual void HandleMessage(RakNetClient client, MessagePacket packet) {
        // Relay the message for now.
        foreach (var player in World.Players)
            player.Send(packet);
    }

    //public virtual void HandleSetTime(RakNetClient client, SetTimePacket packet) { }
    //public virtual void HandleStartGame(RakNetClient client, StartGamePacket packet) { }
    //public virtual void HandleAddMob(RakNetClient client, AddMobPacket packet) { }
    //public virtual void HandleAddPlayer(RakNetClient client, AddPlayerPacket packet) { }
    //public virtual void HandleRemovePlayer(RakNetClient client, RemovePlayerPacket packet) { }
    //public virtual void HandleAddEntity(RakNetClient client, AddEntityPacket packet) { }
    //public virtual void HandleRemoveEntity(RakNetClient client, RemoveEntityPacket packet) { }
    //public virtual void HandleAddItemEntity(RakNetClient client, AddItemEntityPacket packet) { }
    //public virtual void HandleTakeItemEntity(RakNetClient client, TakeItemEntityPacket packet) { }
    //public virtual void HandleMoveEntity(RakNetClient client, MoveEntityPacket packet) { }
    //public virtual void HandleMoveEntityPosRot(RakNetClient client, MoveEntityPosRotPacket packet) { }
    //public virtual void HandleRotateHead(RakNetClient client, RotateHeadPacket packet) { }
    public virtual void HandleMovePlayer(RakNetClient client, MovePlayerPacket packet) => World.MovePlayer(client, packet.Pos, packet.Rot);
    //public virtual void HandlePlaceBlock(RakNetClient client, PlaceBlockPacket packet) { }
    //public virtual void HandleRemoveBlock(RakNetClient client, RemoveBlockPacket packet) { }
    //public virtual void HandleUpdateBlock(RakNetClient client, UpdateBlockPacket packet) { }
    //public virtual void HandleAddPainting(RakNetClient client, AddPaintingPacket packet) { }
    //public virtual void HandleExplode(RakNetClient client, ExplodePacket packet) { }
    //public virtual void HandleLevelEvent(RakNetClient client, LevelEventPacket packet) { }
    //public virtual void HandleTileEvent(RakNetClient client, TileEventPacket packet) { }
    //public virtual void HandleEntityEvent(RakNetClient client, EntityEventPacket packet) { }
    //public virtual void HandleRequestChunk(RakNetClient client, RequestChunkPacket packet) { }
    //public virtual void HandleChunkData(RakNetClient client, ChunkDataPacket packet) { }
    //public virtual void HandlePlayerEquipment(RakNetClient client, PlayerEquipmentPacket packet) { }
    //public virtual void HandlePlayerArmorEquipment(RakNetClient client, PlayerArmorEquipmentPacket packet) { }
    //public virtual void HandleInteract(RakNetClient client, InteractPacket packet) { }
    //public virtual void HandleUseItem(RakNetClient client, UseItemPacket packet) { }
    //public virtual void HandlePlayerAction(RakNetClient client, PlayerActionPacket packet) { }
    //public virtual void HandleHurtArmor(RakNetClient client, HurtArmorPacket packet) { }
    //public virtual void HandleSetEntityData(RakNetClient client, SetEntityDataPacket packet) { }
    //public virtual void HandleSetEntityMotion(RakNetClient client, SetEntityMotionPacket packet) { }
    //public virtual void HandleSetRiding(RakNetClient client, SetRidingPacket packet) { }
    //public virtual void HandleSetHealth(RakNetClient client, SetHealthPacket packet) { }
    //public virtual void HandleSetSpawnPosition(RakNetClient client, SetSpawnPositionPacket packet) { }
    //public virtual void HandleAnimate(RakNetClient client, AnimatePacket packet) { }
    //public virtual void HandleRespawn(RakNetClient client, RespawnPacket packet) { }
    //public virtual void HandleSendInventory(RakNetClient client, SendInventoryPacket packet) { }
    //public virtual void HandleDropItem(RakNetClient client, DropItemPacket packet) { }
    //public virtual void HandleContainerOpen(RakNetClient client, ContainerOpenPacket packet) { }
    //public virtual void HandleContainerClose(RakNetClient client, ContainerClosePacket packet) { }
    //public virtual void HandleContainerSetSlot(RakNetClient client, ContainerSetSlotPacket packet) { }
    //public virtual void HandleContainerSetData(RakNetClient client, ContainerSetDataPacket packet) { }
    //public virtual void HandleContainerSetContent(RakNetClient client, ContainerSetContentPacket packet) { }
    //public virtual void HandleContainerAck(RakNetClient client, ContainerAckPacket packet) { }
    //public virtual void HandleChat(RakNetClient client, ChatPacket packet) { }
    //public virtual void HandleSignUpdate(RakNetClient client, SignUpdatePacket packet) { }
    //public virtual void HandleAdventureSettings(RakNetClient client, AdventureSettingsPacket packet) { }
}