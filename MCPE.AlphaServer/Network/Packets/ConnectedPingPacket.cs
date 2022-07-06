using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ConnectedPingPacket : RakPacket {
        public RakTimestamp TimeSinceStart;

        public ConnectedPingPacket(ref RakDecoder decoder) {
            TimeSinceStart = decoder.Timestamp();
        }

        public override string ToString() => $"ConnectedPing {{ TimeSinceStart: {TimeSinceStart} }}";
    }
}