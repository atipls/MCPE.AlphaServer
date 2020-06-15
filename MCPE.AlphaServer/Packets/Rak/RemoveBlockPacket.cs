using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class RemoveBlockPacket : RakPacket {
        public int ID;
        public int X, Z;
        public byte Y;

        public RemoveBlockPacket(ref RakDecoder decoder) {
            ID = decoder.Int();
            X = decoder.Int();
            Z = decoder.Int();
            Y = decoder.Byte();
        }

        public override string ToString() => $"RemoveBlock {{ Pos: [{X}, {Y}, {Z}], ID: {ID} }}";
    }
}
