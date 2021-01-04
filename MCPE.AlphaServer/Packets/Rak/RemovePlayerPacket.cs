using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class RemovePlayerPacket : RakPacket {
        public int EID;
        public ulong ID;

        public RemovePlayerPacket(Player player) {
            MessageID = RakPacketType.RemovePlayer;
            EID = player.EID;
            ID = player.CID;
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(EID);
            encoder.Encode(ID);

            return encoder.Get();
        }
    }
}
