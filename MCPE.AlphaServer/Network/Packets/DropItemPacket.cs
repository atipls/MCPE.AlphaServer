using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class DropItemPacket : RakPacket {
        public int EID;
        public byte Unknown;
        public short Block;
        public byte Stack;
        public short Meta;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}