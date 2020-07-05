using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class RotateHeadPacket : RakPacket {
        public int EID;
        public byte Yaw;

        public RotateHeadPacket() {
            MessageID = RakPacketType.RotateHead;
        }

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
