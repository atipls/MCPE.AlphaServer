using System;
using System.Collections;
using System.Linq;
using System.Net;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.RakNet;

public enum ConnectedPacketType : byte {
    ConnectedPing = 0x0,
    ConnectedPong = 0x3,
    ConnectionRequest = 0x9,
    ConnectionRequestAccepted = 0x10,
    NewIncomingConnection = 0x13,
    PlayerDisconnect = 0x15
}

public class ConnectedPacket {
    public const int UNRELIABLE = 0;
    public const int UNRELIABLE_SEQUENCED = 1;
    public const int RELIABLE = 2;
    public const int RELIABLE_ORDERED = 3;
    public const int RELIABLE_SEQUENCED = 4;

    internal byte Type;
    internal int OrderingChannel;
    internal int OrderingIndex;
    internal int ReliableIndex;

    internal int Reliability;

    public ConnectedPacket() {
        Reliability = UNRELIABLE;
        ReliableIndex = 0;
        OrderingIndex = 0;
        OrderingChannel = 0;
    }

    public static ConnectedPacket Parse(ref DataReader reader) {
        var flags = reader.Byte();
        var reliability = (flags & 0xE0) >> 5;

        var payloadLength = reader.Short();
        var (reliableIndex, orderingIndex, orderingChannel) = reliability switch {
            RELIABLE => (reader.Triad(), 0, 0),
            RELIABLE_ORDERED => (reader.Triad(), reader.Triad(), reader.Byte()),
            RELIABLE_SEQUENCED => (reader.Triad(), reader.Triad(), 0),
            _ => (0, 0, 0)
        };

        var payload = reader.Read(payloadLength / 8);
        ConnectedPacket packet = payload.Span[0] switch {
            (int) ConnectedPacketType.ConnectedPing => new ConnectedPingPacket(),
            (int) ConnectedPacketType.ConnectedPong => new ConnectedPongPacket(),
            (int) ConnectedPacketType.ConnectionRequest => new ConnectionRequestPacket(),
            (int) ConnectedPacketType.ConnectionRequestAccepted => new ConnectionRequestAcceptedPacket(),
            (int) ConnectedPacketType.NewIncomingConnection => new NewIncomingConnectionPacket(),
            (int) ConnectedPacketType.PlayerDisconnect => new PlayerDisconnectPacket(),
            > 0x80 => new UserPacket(payload),
            _ => null,
        };

        if (packet is null) return null;

        packet.Reliability = reliability;
        packet.ReliableIndex = reliableIndex;
        packet.OrderingIndex = orderingIndex;
        packet.OrderingChannel = orderingChannel;

        var payloadReader = new DataReader(payload);
        packet.Decode(ref payloadReader);

        return packet;
    }

    public static ConnectedMetaPacket ParseMeta(ref DataReader reader) {
        var metaPacket = new ConnectedMetaPacket();
        metaPacket.Decode(ref reader);
        return metaPacket;
    }

    public virtual void Decode(ref DataReader reader) => Type = reader.Byte();
    public virtual void Encode(ref DataWriter writer) => writer.Byte(Type);

    public override string ToString() =>
        $"ConnectedPacket(Reliability={Reliability}{Reliability switch {RELIABLE => $", ReliableIndex={ReliableIndex}", RELIABLE_SEQUENCED => $", ReliableIndex={ReliableIndex}, OrderingIndex={OrderingIndex}, OrderingChannel={OrderingChannel}", _ => ""}})";
}

public class ConnectedMetaPacket : ConnectedPacket, IEnumerable {
    public bool IsACK { get; set; }
    public (int Min, int Max)[] Ranges { get; set; }

    public override void Decode(ref DataReader reader) {
        IsACK = (reader.Byte() & UnconnectedPacket.IS_ACK) != 0;

        var rangeCount = reader.Short();
        Ranges = new (int Min, int Max)[rangeCount];
        for (var i = 0; i < rangeCount; i++) {
            var minIsMax = reader.Byte() != 0;
            if (minIsMax) {
                var sequence = reader.Triad();
                Ranges[i] = (sequence, sequence);
            }
            else Ranges[i] = (reader.Triad(), reader.Triad());
        }
    }

    public override void Encode(ref DataWriter writer) {
        writer.Byte(IsACK ? (byte) UnconnectedPacket.IS_ACK : (byte) UnconnectedPacket.IS_NAK);

        writer.Short((short) Ranges.Length);
        foreach (var (min, max) in Ranges) {
            if (min == max) {
                writer.Byte(1);
                writer.Triad(min);
            }
            else {
                writer.Byte(0);
                writer.Triad(min);
                writer.Triad(max);
            }
        }
    }

    public IEnumerator GetEnumerator() =>
        Ranges.Select(range => Enumerable.Range(range.Min, range.Max)).GetEnumerator();

    public override string ToString() =>
        $"ConnectedMetaPacket(IsACK={IsACK}, Ranges={string.Join(", ", Ranges.Select(range => $"{range.Min}-{range.Max}"))})";
}

public class ConnectedPingPacket : ConnectedPacket {
    public ulong TimeSinceStart;

    public ConnectedPingPacket() => Type = (int) ConnectedPacketType.ConnectedPing;

    public override void Decode(ref DataReader reader) {
        base.Decode(ref reader);

        TimeSinceStart = reader.ULong();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);

        writer.ULong(TimeSinceStart);
    }

    public override string ToString() => $"ConnectedPingPacket(TimeSinceStart={TimeSinceStart})";
}

public class ConnectedPongPacket : ConnectedPacket {
    public ulong TimeSinceStart;
    public ulong TimeSinceServerStart;

    public ConnectedPongPacket() => Type = (int) ConnectedPacketType.ConnectedPong;

    public override void Decode(ref DataReader reader) {
        base.Decode(ref reader);

        TimeSinceStart = reader.ULong();
        TimeSinceServerStart = reader.ULong();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);

        writer.ULong(TimeSinceStart);
        writer.ULong(TimeSinceServerStart);
    }

    public override string ToString() =>
        $"ConnectedPingPacket(TimeSinceStart={TimeSinceStart}, TimeSinceServerStart={TimeSinceServerStart})";
}

public class ConnectionRequestPacket : ConnectedPacket {
    public ulong ClientID;
    public ulong TimeSinceStart;
    public byte UseEncryption;

    public ConnectionRequestPacket() => Type = (int) ConnectedPacketType.ConnectionRequest;

    public override void Decode(ref DataReader reader) {
        base.Decode(ref reader);

        ClientID = reader.ULong();
        TimeSinceStart = reader.ULong();
        UseEncryption = reader.Byte();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);

        writer.ULong(ClientID);
        writer.ULong(TimeSinceStart);
        writer.Byte(UseEncryption);
    }

    public override string ToString() =>
        $"ConnectionRequestPacket(ClientID={ClientID}, TimeSinceStart={TimeSinceStart}, UseEncryption={UseEncryption})";
}

public class ConnectionRequestAcceptedPacket : ConnectedPacket {
    public IPEndPoint EndPoint;
    public ulong TimeSinceStart;

    public ConnectionRequestAcceptedPacket() => Type = (int) ConnectedPacketType.ConnectionRequestAccepted;

    public override void Decode(ref DataReader reader) {
        base.Decode(ref reader);

        EndPoint = reader.IPEndPoint();
        reader.Short(); // SystemIndex

        // RakNet addresses
        for (var i = 0; i < 10; i++)
            reader.IPEndPoint();

        TimeSinceStart = reader.ULong();
        reader.ULong(); // Another time since start
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);

        writer.IPEndPoint(EndPoint);
        writer.Short(0); // SystemIndex

        writer.IPEndPoint(new IPEndPoint(IPAddress.Loopback, 0));
        // RakNet addresses
        var defaultEndPoint = new IPEndPoint(IPAddress.Any, 0);
        for (var i = 0; i < 9; i++)
            writer.IPEndPoint(defaultEndPoint);

        writer.ULong(TimeSinceStart);
        writer.ULong(0); // Another time since start
    }

    public override string ToString() =>
        $"ConnectionRequestAcceptedPacket(EndPoint={EndPoint}, TimeSinceStart={TimeSinceStart})";
}

public class NewIncomingConnectionPacket : ConnectedPacket {
    public IPEndPoint EndPoint;
    public ulong TimeSinceStart1;
    public ulong TimeSinceStart2;

    public NewIncomingConnectionPacket() => Type = (int) ConnectedPacketType.NewIncomingConnection;

    public override void Decode(ref DataReader reader) {
        base.Decode(ref reader);

        EndPoint = reader.IPEndPoint();

        // Internal addresses
        for (var i = 0; i < 10; i++)
            reader.IPEndPoint();

        TimeSinceStart1 = reader.ULong();
        TimeSinceStart2 = reader.ULong();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);

        writer.IPEndPoint(EndPoint);

        // Internal addresses
        var defaultEndPoint = new IPEndPoint(IPAddress.Any, 0);
        for (var i = 0; i < 9; i++)
            writer.IPEndPoint(defaultEndPoint);

        writer.ULong(TimeSinceStart1);
        writer.ULong(TimeSinceStart2);
    }

    public override string ToString() =>
        $"NewIncomingConnectionPacket(EndPoint={EndPoint}, TimeSinceStart1={TimeSinceStart1}, TimeSinceStart2={TimeSinceStart2})";
}

public class PlayerDisconnectPacket : ConnectedPacket {
    public override string ToString() => "PlayerDisconnectPacket()";
}

public class UserPacket : ConnectedPacket {
    public Memory<byte> Data;

    public UserPacket() => Data = new Memory<byte>();
    public UserPacket(Memory<byte> data) => Data = data;

    public override void Decode(ref DataReader reader) { }
    public override void Encode(ref DataWriter writer) { }

    public override string ToString() => $"UserPacket(Data={Data.Length}bytes)";
}