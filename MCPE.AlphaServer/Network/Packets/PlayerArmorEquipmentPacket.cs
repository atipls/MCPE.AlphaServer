using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class PlayerArmorEquipmentPacket : RakPacket {
        public int EID;
        public byte Slot1;
        public byte Slot2;
        public byte Slot3;
        public byte Slot4;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}