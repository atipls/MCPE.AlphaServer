using System.Net;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.RakNet;

internal enum UnconnectedPacketType : byte {
    UnconnectedPing = 0x01,
    UnconnectedPong = 0x1C,

    OpenConnectionRequest1 = 0x05,
    OpenConnectionReply1 = 0x06,
    OpenConnectionRequest2 = 0x07,
    OpenConnectionReply2 = 0x08
}

internal abstract class UnconnectedPacket {
    public const int IS_CONNECTED = 1 << 7;
    public const int IS_ACK = 1 << 6;
    public const int IS_NAK = 1 << 5;
    public const int IS_PAIR = 1 << 4;
    public const int IS_CONTINOUS = 1 << 3;
    public const int NEEDS_B_AS = 1 << 2;

    public static readonly byte[] RakNetMagic =
        { 0x00, 0xFF, 0xFF, 0x00, 0xFE, 0xFE, 0xFE, 0xFE, 0xFD, 0xFD, 0xFD, 0xFD, 0x12, 0x34, 0x56, 0x78 };

    protected UnconnectedPacketType Type { get; set; }

    protected virtual void Decode(ref DataReader reader) => Type = (UnconnectedPacketType)reader.Byte();
    public virtual void Encode(ref DataWriter writer) => writer.Byte((byte)Type);

    public static UnconnectedPacket Parse(byte[] data) {
        var reader = new DataReader(data);
        var type = data[0];

        UnconnectedPacket packet = (UnconnectedPacketType)type switch {
            UnconnectedPacketType.UnconnectedPing => new UnconnectedPingPacket(),
            UnconnectedPacketType.UnconnectedPong => new UnconnectedPongPacket(),
            UnconnectedPacketType.OpenConnectionRequest1 => new OpenConnectionRequest1Packet(),
            UnconnectedPacketType.OpenConnectionRequest2 => new OpenConnectionRequest2Packet(),
            _ => null
        };

        if (packet == null) return null;

        packet.Decode(ref reader);
        return packet;
    }
}

internal class UnconnectedPingPacket : UnconnectedPacket {
    public ulong TimeSinceStart;

    public UnconnectedPingPacket() {
        Type = UnconnectedPacketType.UnconnectedPing;
        TimeSinceStart = 0;
    }

    protected override void Decode(ref DataReader reader) {
        base.Decode(ref reader);

        TimeSinceStart = reader.ULong();
        reader.RakNetMagic();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);

        writer.ULong(TimeSinceStart);
        writer.RakNetMagic();
    }

    public override string ToString() {
        return $"UnconnectedPing(TimeSinceStart={TimeSinceStart})";
    }
}

internal class UnconnectedPongPacket : UnconnectedPacket {
    public string Data;
    public ulong Guid;
    public ulong TimeSinceStart;

    public UnconnectedPongPacket() {
        Type = UnconnectedPacketType.UnconnectedPong;
        TimeSinceStart = 0;
        Guid = 0;
        Data = "";
    }

    public UnconnectedPongPacket(ulong timeSinceStart, ulong guid, string data) {
        Type = UnconnectedPacketType.UnconnectedPong;
        TimeSinceStart = timeSinceStart;
        Guid = guid;
        Data = data;
    }

    protected override void Decode(ref DataReader reader) {
        base.Decode(ref reader);

        TimeSinceStart = reader.ULong();
        Guid = reader.ULong();
        reader.RakNetMagic();
        Data = reader.String();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);

        writer.ULong(TimeSinceStart);
        writer.ULong(Guid);
        writer.RakNetMagic();
        writer.String(Data);
    }


    public override string ToString() => $"UnconnectedPong(TimeSinceStart={TimeSinceStart}, Guid={Guid}, Data={Data})";
}

internal class OpenConnectionRequest1Packet : UnconnectedPacket {
    public byte ProtocolVersion;

    public OpenConnectionRequest1Packet() {
        Type = UnconnectedPacketType.OpenConnectionRequest1;
        ProtocolVersion = 0;
    }

    public OpenConnectionRequest1Packet(byte protocolVersion) {
        Type = UnconnectedPacketType.OpenConnectionRequest1;
        ProtocolVersion = protocolVersion;
    }

    protected override void Decode(ref DataReader reader) {
        base.Decode(ref reader);
        reader.RakNetMagic();

        ProtocolVersion = reader.Byte();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);
        writer.RakNetMagic();

        writer.Byte(ProtocolVersion);
    }

    public override string ToString() => $"OpenConnectionRequest1(ProtocolVersion={ProtocolVersion})";
}

internal class OpenConnectionRequest2Packet : UnconnectedPacket {
    public ulong ClientID;
    public IPEndPoint EndPoint;
    public ushort MtuSize;

    public OpenConnectionRequest2Packet() {
        Type = UnconnectedPacketType.OpenConnectionRequest2;
        EndPoint = new IPEndPoint(IPAddress.Any, 0);
        MtuSize = 0;
        ClientID = 0;
    }

    public OpenConnectionRequest2Packet(IPEndPoint endPoint, ushort mtuSize, ulong clientID) {
        Type = UnconnectedPacketType.OpenConnectionRequest2;
        EndPoint = endPoint;
        MtuSize = mtuSize;
        ClientID = clientID;
    }

    protected override void Decode(ref DataReader reader) {
        base.Decode(ref reader);
        reader.RakNetMagic();

        EndPoint = reader.IPEndPoint();
        MtuSize = reader.UShort();
        ClientID = reader.ULong();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);
        writer.RakNetMagic();

        writer.IPEndPoint(EndPoint);
        writer.UShort(MtuSize);
        writer.ULong(ClientID);
    }

    public override string ToString() =>
        $"OpenConnectionRequest2(EndPoint={EndPoint}, MtuSize={MtuSize}, ClientID={ClientID})";
}

internal class OpenConnectionReply1Packet : UnconnectedPacket {
    public ushort MtuSize;
    public ulong ServerID;
    public byte UseEncryption;

    public OpenConnectionReply1Packet() {
        Type = UnconnectedPacketType.OpenConnectionReply1;
        ServerID = 0;
        UseEncryption = 0;
        MtuSize = 0;
    }

    public OpenConnectionReply1Packet(ulong serverID, bool useEncryption, ushort mtuSize) {
        Type = UnconnectedPacketType.OpenConnectionReply1;
        ServerID = serverID;
        UseEncryption = (byte)(useEncryption ? 0x01 : 0x00);
        MtuSize = mtuSize;
    }

    protected override void Decode(ref DataReader reader) {
        base.Decode(ref reader);
        reader.RakNetMagic();

        ServerID = reader.ULong();
        UseEncryption = reader.Byte();
        MtuSize = reader.UShort();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);
        writer.RakNetMagic();

        writer.ULong(ServerID);
        writer.Byte(UseEncryption);
        writer.UShort(MtuSize);
    }

    public override string ToString() =>
        $"OpenConnectionReply1(ServerID={ServerID}, UseEncryption={UseEncryption}, MtuSize={MtuSize})";
}

internal class OpenConnectionReply2Packet : UnconnectedPacket {
    public IPEndPoint ClientAddress;
    public ushort MtuSize;
    public ulong ServerID;
    public byte UseEncryption;

    public OpenConnectionReply2Packet() {
        Type = UnconnectedPacketType.OpenConnectionReply2;
        ServerID = 0;
        ClientAddress = new IPEndPoint(IPAddress.Any, 0);
        MtuSize = 0;
        UseEncryption = 0;
    }

    public OpenConnectionReply2Packet(ulong serverID, IPEndPoint clientAddress, ushort mtuSize, bool useEncryption) {
        Type = UnconnectedPacketType.OpenConnectionReply2;
        ServerID = serverID;
        ClientAddress = clientAddress;
        MtuSize = mtuSize;
        UseEncryption = (byte)(useEncryption ? 0x01 : 0x00);
    }

    protected override void Decode(ref DataReader reader) {
        base.Decode(ref reader);
        reader.RakNetMagic();

        ServerID = reader.ULong();
        ClientAddress = reader.IPEndPoint();
        MtuSize = reader.UShort();
        UseEncryption = reader.Byte();
    }

    public override void Encode(ref DataWriter writer) {
        base.Encode(ref writer);
        writer.RakNetMagic();

        writer.ULong(ServerID);
        writer.IPEndPoint(ClientAddress);
        writer.UShort(MtuSize);
        writer.Byte(UseEncryption);
    }

    public override string ToString() =>
        $"OpenConnectionReply2(ServerID={ServerID}, ClientAddress={ClientAddress}, MtuSize={MtuSize}, UseEncryption={UseEncryption})";
}
