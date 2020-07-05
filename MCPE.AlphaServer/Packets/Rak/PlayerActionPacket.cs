using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class PlayerActionPacket : RakPacket {
        public int Action;
        public int X, Y, Z;
        public int Face;
        public int EID;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
