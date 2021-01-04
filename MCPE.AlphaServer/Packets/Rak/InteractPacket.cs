using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class InteractPacket : RakPacket {
        public byte Action;
        public int EID;
        public int Target;

        public InteractPacket(ref RakDecoder decoder) {
            Action = decoder.Byte();
            EID = decoder.Int();
            Target = decoder.Int();
        }

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
