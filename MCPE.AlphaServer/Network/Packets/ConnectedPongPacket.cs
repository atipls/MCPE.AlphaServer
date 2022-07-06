using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ConnectedPongPacket : RakPacket {
        public RakTimestamp TimeSinceStart;
        public RakTimestamp TimeSinceServerStart;

        public static ConnectedPongPacket FromPing(ConnectedPingPacket packet, DateTime start) {
            return new ConnectedPongPacket() {
                MessageFlags = packet.MessageFlags,
                ReliableNum = packet.ReliableNum,
                OrderingIndex = packet.OrderingIndex,
                OrderingChannel = packet.OrderingChannel,
                MessageID = RakPacketType.ConnectedPong,
                TimeSinceStart = packet.TimeSinceStart,
                TimeSinceServerStart = new RakTimestamp((ulong) (DateTime.Now - start).TotalMilliseconds),
            };
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(TimeSinceStart);
            encoder.Encode(TimeSinceServerStart);

            return encoder.Get();
        }
    }
}