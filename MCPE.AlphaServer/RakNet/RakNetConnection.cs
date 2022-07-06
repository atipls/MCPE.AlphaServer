using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.RakNet;

public class RakNetConnection {
    public enum ConnectionStatus {
        CONNECTING,
        CONNECTED,
        DISCONNECTING,
        DISCONNECTED
    }

    public RakNetConnection(IPEndPoint endPoint, RakNetServer server) {
        IP = endPoint;
        LastPing = DateTime.Now;
        Status = ConnectionStatus.CONNECTING;
        OutgoingPackets = new List<ConnectedPacket>();
        NeedsACK = new Dictionary<int, bool>();
        CurrentSequenceNumber = 0;
        LastReliablePacketIndex = 0;
        Server = server;
    }

    public IPEndPoint IP { get; init; }
    public DateTime LastPing { get; set; }
    public ulong ClientID { get; set; }
    public ConnectionStatus Status { get; set; }

    private List<ConnectedPacket> OutgoingPackets { get; }
    private Dictionary<int, bool> NeedsACK { get; }
    private int CurrentSequenceNumber;
    private int LastReliablePacketIndex;
    
    internal RakNetServer Server;

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

        return Enumerable.Empty<ReadOnlyMemory<byte>>();
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
        NeedsACK[sequenceNumber] = true;

        List<ReadOnlyMemory<byte>> packets = new();
        do {
            var packet = ConnectedPacket.Parse(ref reader);
    
            if (packet.Reliability >= ConnectedPacket.RELIABLE)
                LastReliablePacketIndex = packet.ReliableIndex;

            switch (packet) {
                case ConnectedPingPacket ping:
                    Logger.Debug($"{IP} Ping: {ping}");
                    break;
                case ConnectionRequestPacket:
                    Send(new ConnectionRequestAcceptedPacket {
                        EndPoint = IP,
                        TimeSinceStart = 0 // TODO: Fix.
                    }, ConnectedPacket.RELIABLE);
                    break;
                case NewIncomingConnectionPacket incoming:
                    Logger.Debug($"{IP} Incoming: {incoming}");
                    break;
                case UserPacket user:
                    Logger.Debug($"{IP} User: {user}");
                    break;
                default:
                    Logger.Warn($"Unhandled {packet}?");
                    break;
            }
        } while (!reader.IsEof);

        return packets;
    }

    internal async Task HandleOutgoing() {
        if (OutgoingPackets.Count < 1)
            return;

        var writer = new DataWriter();

        writer.Byte(UnconnectedPacket.IS_CONNECTED);  // TODO: Split packets?
        writer.Triad(CurrentSequenceNumber++);

        foreach (var packet in OutgoingPackets) {
            var packetWriter = new DataWriter();
            packet.Encode(ref packetWriter);

            writer.Byte((byte)(packet.Reliability << 5));
            writer.Short((short)(packetWriter.Length * 8));

            switch (packet.Reliability) {
                case ConnectedPacket.RELIABLE:
                    writer.Triad(packet.ReliableIndex);
                    break;
                case ConnectedPacket.RELIABLE_ORDERED:
                    writer.Triad(packet.ReliableIndex);
                    writer.Triad(packet.OrderingIndex);
                    writer.Byte((byte) packet.OrderingChannel);
                    break;
            }

            writer.RawData(packetWriter.GetBytes());
        }

        await Server.UDP.SendAsync(writer.GetBytes(), IP);
        OutgoingPackets.Clear();
    }

    public void Send(ConnectedPacket packet, int reliability) {
        if (reliability == ConnectedPacket.RELIABLE) {
            packet.Reliability = reliability;
            packet.ReliableIndex = LastReliablePacketIndex++;
        }

        OutgoingPackets.Add(packet);
    }

    public override string ToString() => $"RakNetConnection(IP={IP}, LastPing={LastPing}, ClientID={ClientID})";
}