using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class UpdateBlockPacket : RakPacket {
        public int EID;
        public int X, Z;
        public byte Y;
        public byte Block;
        public byte Meta;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}