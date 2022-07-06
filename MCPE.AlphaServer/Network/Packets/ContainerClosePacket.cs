using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ContainerClosePacket : RakPacket {
        public byte WindowID;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}