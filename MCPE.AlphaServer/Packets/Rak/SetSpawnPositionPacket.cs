using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class SetSpawnPositionPacket : RakPacket {
        public int X, Z;
        public byte Y;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
