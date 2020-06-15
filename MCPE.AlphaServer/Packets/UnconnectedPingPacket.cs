using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.Packets {
    public class UnconnectedPingPacket : Packet {
        public RakTimestamp TimeSinceStart;

        public UnconnectedPingPacket(byte[] data) {
            RakDecoder decoder = new RakDecoder(data);

            Type = (PacketType)decoder.Byte();
            TimeSinceStart = decoder.Timestamp();
            decoder.Magic();
        }

        public override string ToString() => $"UnconnectedPingPacket {{ TimeSinceStart: {TimeSinceStart} }}";
    }
}
