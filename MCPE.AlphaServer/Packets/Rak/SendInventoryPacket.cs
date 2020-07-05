using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class SendInventoryPacket : RakPacket {
        public int EID;
        public byte WindowID;
        public short Count;
        public byte[] Slots;
        public byte[] Items;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
