using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ReadyPacket : RakPacket {
        public byte Status;

        public ReadyPacket(ref RakDecoder decoder) {
            Status = decoder.Byte();
        }

        public override string ToString() => $"Ready {{ Status: {Status:X} }}";
    }
}