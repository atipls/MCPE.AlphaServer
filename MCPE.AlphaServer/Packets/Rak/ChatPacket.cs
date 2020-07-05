using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ChatPacket : RakPacket {
        public string Message;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
