using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class InteractPacket : RakPacket {
        public byte[] Action; // NOTE(atipls): 8 bits (?)
        public int EID;
        public int Target;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
