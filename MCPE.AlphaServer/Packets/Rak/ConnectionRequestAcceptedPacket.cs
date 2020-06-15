using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;

namespace MCPE.AlphaServer.Packets {
    public class ConnectionRequestAcceptedPacket : RakPacket {
        public RakTimestamp TimeSinceStart;
        public RakAddress Client;
        public short SystemIndex;

        public static ConnectionRequestAcceptedPacket FromRequest(ConnectionRequestPacket packet, IPEndPoint client) {
            return new ConnectionRequestAcceptedPacket {
                MessageFlags = packet.MessageFlags,
                ReliableNum = packet.ReliableNum.Add(1),
                OrderingIndex = packet.OrderingIndex,
                OrderingChannel = packet.OrderingChannel,
                MessageID = RakPacketType.ConnectionRequestAccepted,
                TimeSinceStart = packet.TimeSinceStart,
                Client = new RakAddress(client),
            };
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(Client);
            encoder.Encode((short)0); // SystemIndex
            encoder.Encode(new RakAddress(IPAddress.Loopback, 0));
            for (int i = 0; i < 9; i++)
                encoder.Encode(new RakAddress(IPAddress.Any, 0));

            encoder.Encode(TimeSinceStart);
            encoder.Encode(new RakTimestamp(0)); // another time since start

            return encoder.Get();
        }
    }
}
