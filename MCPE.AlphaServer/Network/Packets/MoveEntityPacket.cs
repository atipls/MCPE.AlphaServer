using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class MoveEntityPacket : RakPacket {
        public int EID;
        public float X, Y, Z;

        public MoveEntityPacket() {
            MessageID = RakPacketType.MoveEntity;
        }

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}