using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class MoveEntityPosRotPacket : RakPacket {
        public int EID;
        public float X, Y, Z;
        public float Yaw, Pitch;

        public MoveEntityPosRotPacket() {
            MessageID = RakPacketType.MoveEntityPosRot;
        }

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
