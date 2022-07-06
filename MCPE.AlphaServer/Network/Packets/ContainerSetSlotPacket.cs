using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ContainerSetSlotPacket : RakPacket {
        public byte WindowID;
        public short Slot;
        public short Block;
        public byte Stack;
        public short Meta;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}