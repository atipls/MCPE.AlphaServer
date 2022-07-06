using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class TileEventPacket : RakPacket {
        public int X, Y, Z;
        public int Case1, Case2;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}