using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class AddEntityPacket : RakPacket {
        public int EID;
        public byte Type;
        public float X, Y, Z;
        public int Did;
        public short SpeedX, SpeedY, SpeedZ;
        public AddEntityPacket() {
            MessageID = RakPacketType.AddEntity;
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(EID);
            encoder.Encode(Type);
            encoder.Encode(X);
            encoder.Encode(Y);
            encoder.Encode(Z);
            encoder.Encode(Did);
            if (Did > 0) {
                encoder.Encode(SpeedX);
                encoder.Encode(SpeedY);
                encoder.Encode(SpeedZ);
            }

            return encoder.Get();
        }
    }
}
