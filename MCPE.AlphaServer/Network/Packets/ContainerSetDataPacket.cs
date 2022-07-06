using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ContainerSetDataPacket : RakPacket {
        public byte WindowID;
        public short Property;
        public short Value;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}