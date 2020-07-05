using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ChunkDataPacket : RakPacket {
        public int X, Z;
        public byte IsNew;
        public byte[] Data;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
