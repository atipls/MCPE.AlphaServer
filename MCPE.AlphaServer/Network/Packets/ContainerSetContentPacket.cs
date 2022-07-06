using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ContainerSetContentPacket : RakPacket {
        public byte WindowID;
        public short Count;
        public byte[] Items;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}