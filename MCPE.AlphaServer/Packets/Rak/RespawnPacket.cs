using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class RespawnPacket : RakPacket {
        public int EID;
        public float X, Y, Z;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
