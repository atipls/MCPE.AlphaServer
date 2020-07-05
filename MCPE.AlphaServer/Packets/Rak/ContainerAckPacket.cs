using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ContainerAckPacket : RakPacket {
        public byte WindowID;
        public short Unknown;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
