using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.RakNet;

public class RakNetConnection {
    public enum ConnectionStatus {
        CONNECTING,
        CONNECTED,
        DISCONNECTING,
        DISCONNECTED
    }

    public RakNetConnection(IPEndPoint endPoint) {
        IP = endPoint;
        LastPing = DateTime.Now;
        Status = ConnectionStatus.CONNECTING;
        OutgoingPackets = new Dictionary<int, ConnectedPacket>();
        NeedsACK = new Dictionary<int, ConnectedPacket>();
    }

    public IPEndPoint IP { get; init; }
    public DateTime LastPing { get; set; }
    public ulong ClientID { get; set; }
    public ConnectionStatus Status { get; set; }

    private Dictionary<int, ConnectedPacket> OutgoingPackets { get; }
    private Dictionary<int, ConnectedPacket> NeedsACK { get; }

    public bool IsTimedOut => DateTime.Now - LastPing > TimeSpan.FromSeconds(5);
    public bool IsConnected => !IsTimedOut && Status != ConnectionStatus.DISCONNECTED;

    internal IEnumerable<ReadOnlyMemory<byte>> HandlePacket(byte[] data) {
        LastPing = DateTime.Now;

        Logger.Debug(
            $"{IP} PreProcess: IsACK={data[0] & UnconnectedPacket.IS_ACK}, IsNAK={data[0] & UnconnectedPacket.IS_NAK}, IsConnected={data[0] & UnconnectedPacket.IS_CONNECTED}");
        Logger.Debug(Formatters.AsHex(data));

        var reader = new DataReader(data);
        if ((data[0] & UnconnectedPacket.IS_ACK) != 0)
            HandleACK(ref reader);
        else if ((data[0] & UnconnectedPacket.IS_NAK) != 0)
            HandleNAK(ref reader);
        else if ((data[0] & UnconnectedPacket.IS_CONNECTED) != 0)
            return HandleConnected(ref reader);

        return null;
    }

    private void HandleACK(ref DataReader reader) {
        var packet = ConnectedPacket.ParseMeta(ref reader);
        Logger.Warn($"TODO: HandleACK {packet}");
    }

    private void HandleNAK(ref DataReader reader) {
        var packet = ConnectedPacket.ParseMeta(ref reader);
        Logger.Warn($"TODO: HandleNAK {packet}");
    }

    private IEnumerable<ReadOnlyMemory<byte>> HandleConnected(ref DataReader reader) {
        reader.Byte();
        var sequenceNumber = reader.Triad();

        List<ReadOnlyMemory<byte>> packets = new();
        do {
            switch (ConnectedPacket.Parse(ref reader)) {
                case ConnectedPingPacket ping:
                    Logger.Debug($"{IP} Ping: {ping}");
                    break;
                case ConnectionRequestPacket request:
                    Logger.Debug($"{IP} Request: {request}");
                    break;
                case NewIncomingConnectionPacket incoming:
                    Logger.Debug($"{IP} Incoming: {incoming}");
                    break;
                case UserPacket user:
                    Logger.Debug($"{IP} User: {user}");
                    break;
                case { } packet:
                    Logger.Warn($"Unhandled {packet}?");
                    break;
            }
        } while (!reader.IsEof);

        return packets;
    }


    public override string ToString() => $"RakNetConnection(IP={IP}, LastPing={LastPing}, ClientID={ClientID})";
}