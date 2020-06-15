using System;
using System.Collections.Generic;
using System.Text;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.Packets {
    public class UnconnectedPongPacket : Packet {
        public RakTimestamp TimeSinceStart;
        public ulong Guid;
        public string Data;

        public static UnconnectedPongPacket FromPing(UnconnectedPingPacket packet, ulong guid, string name) {
            return new UnconnectedPongPacket() {
                Type = PacketType.UnconnectedPong,
                TimeSinceStart = packet.TimeSinceStart,
                Guid = guid,
                Data = "MCCPP;Demo;" + name,
            };
        }

        public override byte[] Serialize() {
            RakEncoder encoder = new RakEncoder();

            encoder.Encode((byte)Type);
            encoder.Encode(TimeSinceStart);
            encoder.Encode(Guid.Signed());
            encoder.AddMagic();
            encoder.Encode(Data);

            return encoder.Get();
        }
    }
}
