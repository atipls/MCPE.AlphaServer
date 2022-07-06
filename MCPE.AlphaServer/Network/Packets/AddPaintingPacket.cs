using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class AddPaintingPacket : RakPacket {
        public int EID;
        public int X, Y, Z;
        public int Direction;
        public string Title;

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}