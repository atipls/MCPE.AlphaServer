using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class SetEntityDataPacket : RakPacket {
        public int EID;
        public byte[] Metadata;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
