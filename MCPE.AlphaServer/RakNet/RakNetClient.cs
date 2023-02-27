using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.RakNet;

public class RakNetClient {
    public enum ConnectionStatus {
        CONNECTING,
        CONNECTED,
        DISCONNECTING,
        DISCONNECTED
    }

    public RakNetClient(IPEndPoint endPoint, RakNetServer server) {
        IP = endPoint;
        LastPing = DateTime.Now;
        Status = ConnectionStatus.CONNECTING;
        OutgoingPackets = new List<ConnectedPacket>();
        NeedsACK = new SortedSet<int>();
        CurrentSequenceNumber = 0;
        LastReliablePacketIndex = 0;
        Server = server;
    }

    public IPEndPoint IP { get; init; }
    public DateTime LastPing { get; set; }
    public ulong ClientID { get; set; }
    public ConnectionStatus Status { get; set; }

    private List<ConnectedPacket> OutgoingPackets { get; }
    private SortedSet<int> NeedsACK { get; }
    private int CurrentSequenceNumber;
    private int LastReliablePacketIndex;

    internal RakNetServer Server;

    public bool IsTimedOut => DateTime.Now - LastPing > TimeSpan.FromSeconds(5);
    public bool IsConnected => !IsTimedOut && Status != ConnectionStatus.DISCONNECTED;

    internal void HandlePacket(byte[] data) {
        LastPing = DateTime.Now;

        // Logger.Debug(
        //        $"{IP} PreProcess: IsACK={data[0] & UnconnectedPacket.IS_ACK}, IsNAK={data[0] & UnconnectedPacket.IS_NAK}, IsConnected={data[0] & UnconnectedPacket.IS_CONNECTED}");
        // Logger.Debug(Formatters.AsHex(data));

        var reader = new DataReader(data);
        if ((data[0] & UnconnectedPacket.IS_ACK) != 0)
            HandleACK(ref reader);
        else if ((data[0] & UnconnectedPacket.IS_NAK) != 0)
            HandleNAK(ref reader);
        else if ((data[0] & UnconnectedPacket.IS_CONNECTED) != 0)
            HandleConnected(ref reader);
    }

    private void HandleACK(ref DataReader reader) {
        var packet = ConnectedPacket.ParseMeta(ref reader);
        //Logger.Warn($"TODO: HandleACK {packet}");
    }

    private void HandleNAK(ref DataReader reader) {
        var packet = ConnectedPacket.ParseMeta(ref reader);
        Logger.Warn($"TODO: HandleNAK {packet}");
    }

    private void HandleConnected(ref DataReader reader) {
        reader.Byte();
        var sequenceNumber = reader.Triad();
        NeedsACK.Add(sequenceNumber);

        do {
            switch (ConnectedPacket.Parse(ref reader)) {
                case ConnectedPingPacket ping:
                    Send(new ConnectedPongPacket {
                            TimeSinceStart = ping.TimeSinceStart,
                            TimeSinceServerStart = 0,
                        }, ConnectedPacket.RELIABLE
                    );
                    break;
                case ConnectionRequestPacket:
                    Send(new ConnectionRequestAcceptedPacket {
                            EndPoint = IP,
                            TimeSinceStart = 0 // TODO: Fix.
                        }, ConnectedPacket.RELIABLE
                    );
                    break;
                case NewIncomingConnectionPacket:
                    Status = ConnectionStatus.CONNECTED;
                    Server.OnOpen(this);
                    break;
                case UserPacket user:
                    Server.OnData(this, user.Data);
                    break;
                case PlayerDisconnectPacket:
                    Status = ConnectionStatus.DISCONNECTED;
                    break;
                case { } packet:
                    Logger.Warn($"Unhandled {packet}?");
                    break;
            }
        } while (!reader.IsEof);
    }

    internal async Task HandleOutgoing() {
        if (OutgoingPackets.Count < 1) {
            if (NeedsACK.Count < 1)
                return;

            // Send ACKs.
            var ackWriter = new DataWriter();
            ackWriter.Byte(UnconnectedPacket.IS_CONNECTED | UnconnectedPacket.IS_ACK);

            // TODO: Use the range feature from RakNet?
            ackWriter.Short((short)NeedsACK.Count);
            foreach (var sequence in NeedsACK) {
                ackWriter.Byte(1); // Min == max.
                ackWriter.Triad(sequence);
            }

            await Server.UDP.SendAsync(ackWriter.GetBytes(), IP);

            NeedsACK.Clear();
            return;
        }

        var writer = new DataWriter();

        writer.Byte(UnconnectedPacket.IS_CONNECTED); // TODO: Split packets?
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
                    writer.Byte((byte)packet.OrderingChannel);
                    break;
            }

            writer.RawData(packetWriter.GetBytes());
        }

        await Server.UDP.SendAsync(writer.GetBytes(), IP);
        OutgoingPackets.Clear();
    }

    public void Send(ConnectedPacket packet, int reliability = ConnectedPacket.RELIABLE) {
        if (reliability == ConnectedPacket.RELIABLE) {
            packet.Reliability = reliability;
            packet.ReliableIndex = LastReliablePacketIndex++;
        }

        OutgoingPackets.Add(packet);
    }

    public override string ToString() => $"RakNetConnection(IP={IP}, LastPing={LastPing}, ClientID={ClientID})";
}
