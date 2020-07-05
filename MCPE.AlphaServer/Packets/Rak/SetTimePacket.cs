using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class SetTimePacket : RakPacket {
        public int Time;

        public SetTimePacket(int time) {
            MessageID = RakPacketType.SetTime;
            Time = time;
        }
        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(Time);

            return encoder.Get();
        }

        public override string ToString() => $"SetTime {{ Time: {Time} }}";
    }
}
