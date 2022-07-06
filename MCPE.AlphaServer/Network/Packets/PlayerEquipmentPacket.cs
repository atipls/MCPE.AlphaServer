using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class PlayerEquipmentPacket : RakPacket {
        public int EID;
        public short Block;
        public short Meta;
        public byte Slot;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}