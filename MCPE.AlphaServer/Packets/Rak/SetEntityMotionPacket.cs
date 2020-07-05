using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class SetEntityMotionPacket : RakPacket {
        public byte Count;
        public EntityMotionData[] Data;
        public struct EntityMotionData {
            public int EID;
            public int MotX, MotY, MotZ;
        }

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
