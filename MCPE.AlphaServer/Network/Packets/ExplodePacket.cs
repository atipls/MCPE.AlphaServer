using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class ExplodePacket : RakPacket {
        public float X, Y, Z;
        public float Radius;
        public int Count;
        public byte Records;
        public byte _Counts;
        public byte _Records;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}