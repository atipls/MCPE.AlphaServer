using System;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.Packets {
    public class RakPacket {
        public byte MessageFlags;
        public short Length;
        public RakTriad ReliableNum;
        public RakTriad OrderingIndex;
        public byte OrderingChannel;
        public RakPacketType MessageID;

        public const int IS_ORDERED = 0x20;
        public const int IS_RELIABLE = 0x40;

        // RakPacket should always be parsed from RakNetPacket 
        // which provides the RakDecoder as there can be multiple.
        public static RakPacket Parse(ref RakDecoder decoder) {
            var flags = decoder.Byte();
            var length = decoder.Short();
            var relnum = RakTriad.FromInt(0, false);
            var orderidx = RakTriad.FromInt(0, false);
            byte orderch = 0;
            if ((flags & IS_RELIABLE) != 0) { // reliable
                relnum = decoder.Triad();
            }
            if ((flags & IS_ORDERED) != 0) { // ordered
                orderidx = decoder.Triad();
                orderch = decoder.Byte();
            }
            var msgid = (RakPacketType)decoder.Byte();
            var packet = new RakPacket();

            switch (msgid) {
            case RakPacketType.ConnectedPing: packet = new ConnectedPingPacket(ref decoder); break;
            case RakPacketType.ConnectionRequest: packet = new ConnectionRequestPacket(ref decoder); break;
            case RakPacketType.NewIncomingConnection: packet = new NewIncomingConnectionPacket(ref decoder); break;
            case RakPacketType.PlayerDisconnect: break;
            case RakPacketType.LoginRequest: packet = new LoginRequestPacket(ref decoder); break;
            case RakPacketType.Ready: packet = new ReadyPacket(ref decoder); break;
            case RakPacketType.RequestChunk: packet = new RequestChunkPacket(ref decoder); break;
            case RakPacketType.MovePlayer: packet = new MovePlayerPacket(ref decoder); break;
            case RakPacketType.SetHealth: packet = new SetHealthPacket(ref decoder); break;
            case RakPacketType.Animate: packet = new AnimatePacket(ref decoder); break;
            case RakPacketType.RemoveBlock: packet = new RemoveBlockPacket(ref decoder); break;
            case RakPacketType.Message: packet = new MessagePacket(ref decoder); break;
            case RakPacketType.UseItem: packet = new UseItemPacket(ref decoder); break;
            case RakPacketType.Interact: packet = new InteractPacket(ref decoder); break;
            default:
                Console.WriteLine($"[!!] Unhandled RakPacket Type {msgid}!");
                break;
            }
            packet.MessageFlags = flags;
            packet.Length = length;
            packet.ReliableNum = relnum;
            packet.OrderingIndex = orderidx;
            packet.OrderingChannel = orderch;
            packet.MessageID = msgid;
            return packet;
        }
        public T Get<T>() where T : RakPacket => this as T;
        public static byte[] Create(RakPacket packet) {
            var encoder = new RakEncoder();
            var serialized = packet.Serialize();

            encoder.Encode(packet.MessageFlags);
            encoder.Encode((short)((serialized.Length + 1) * 8));
            if ((packet.MessageFlags & IS_RELIABLE) != 0)
                encoder.Encode(packet.ReliableNum);
            if ((packet.MessageFlags & IS_ORDERED) != 0) {
                encoder.Encode(packet.OrderingIndex);
                encoder.Encode(packet.OrderingChannel);
            }
            encoder.Encode((byte)packet.MessageID);
            encoder.Encode(serialized);

            return encoder.Get();
        }

        public virtual byte[] Serialize() => new byte[] { 0 };
        public override string ToString() => $"RakPacket {{ MessageFlags: {MessageFlags}, Length: {Length} bits, ReliableNum: {ReliableNum}, MessageID: {MessageID} }}";
    }
}