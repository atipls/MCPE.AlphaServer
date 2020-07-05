using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class RemoveBlockPacket : RakPacket {
        public int EID;
        public int X, Z;
        public byte Y;

        public RemoveBlockPacket(ref RakDecoder decoder) {
            EID = decoder.Int();
            X = decoder.Int();
            Z = decoder.Int();
            Y = decoder.Byte();
        }

        public override string ToString() => $"RemoveBlock {{ Pos: [{X}, {Y}, {Z}], EID: {EID} }}";
    }
}
