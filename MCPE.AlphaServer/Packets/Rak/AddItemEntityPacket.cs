using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class AddItemEntityPacket : RakPacket {
        public int EID;
        public short Block;
        public byte Stack;
        public short Meta;
        public float X, Y, Z;
        public byte Yaw, Pitch, Roll;
        public AddItemEntityPacket() {
            MessageID = RakPacketType.AddItemEntity;
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(EID);
            encoder.Encode(Block);
            encoder.Encode(Stack);
            encoder.Encode(Meta);
            encoder.Encode(X);
            encoder.Encode(Y);
            encoder.Encode(Z);
            encoder.Encode(Yaw);
            encoder.Encode(Pitch);
            encoder.Encode(Roll);

            return encoder.Get();
        }
    }
}
