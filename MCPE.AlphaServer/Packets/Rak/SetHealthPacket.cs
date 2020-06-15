using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class SetHealthPacket : RakPacket {
        public byte Health;

        public SetHealthPacket(ref RakDecoder decoder) => Health = decoder.Byte();

        public override string ToString() => $"SetHealth {{ Health: {Health} }}";
    }
}
