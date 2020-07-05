using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class PlaceBlockPacket : RakPacket {
        public int EID;
        public int X, Z;
        public byte Y;
        public byte Block;
        public byte Meta;
        public byte Face;

        public PlaceBlockPacket() {
            MessageID = RakPacketType.PlaceBlock;
        }

        public override byte[] Serialize() => throw new NotImplementedException();
    }
}
