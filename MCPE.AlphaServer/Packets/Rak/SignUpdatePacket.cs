using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class SignUpdatePacket : RakPacket {
        public short X;
        public byte Y;
        public short Z;
        public string Lines;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
