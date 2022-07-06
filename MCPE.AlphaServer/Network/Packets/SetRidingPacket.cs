using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class SetRidingPacket : RakPacket {
        public int EID;
        public int Target;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}