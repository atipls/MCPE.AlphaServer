using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class HurtArmorPacket : RakPacket {
        public byte Armor;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
