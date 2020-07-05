using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ContainerOpenPacket : RakPacket {
        public byte WindowID;
        public byte Type;
        public byte Slot;
        public string Title;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
