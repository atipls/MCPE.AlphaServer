using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class LevelEventPacket : RakPacket {
        public short Event;
        public short X, Y, Z;
        public int Data;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
