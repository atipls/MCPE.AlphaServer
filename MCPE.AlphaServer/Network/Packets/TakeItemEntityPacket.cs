using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class TakeItemEntityPacket : RakPacket {
        public int Target;
        public int EID;

        public TakeItemEntityPacket() {
            MessageID = RakPacketType.TakeItemEntity;
        }

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}