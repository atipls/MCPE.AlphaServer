using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class EntityEventPacket : RakPacket {
        public int EID;
        public byte Event;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
