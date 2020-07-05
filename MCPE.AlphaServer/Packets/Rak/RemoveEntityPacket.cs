using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class RemoveEntityPacket : RakPacket {
        public int EID;
        public RemoveEntityPacket() {
            MessageID = RakPacketType.RemoveEntity;
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(EID);

            return encoder.Get();
        }
    }
}
