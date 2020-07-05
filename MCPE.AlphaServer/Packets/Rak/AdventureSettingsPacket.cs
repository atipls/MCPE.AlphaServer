using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class AdventureSettingsPacket : RakPacket {
        public byte[] Flags;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
