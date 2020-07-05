using MCPE.AlphaServer.Game;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class AddMobPacket : RakPacket {
        public int EID;
        public int Type;
        public float X, Y, Z;
        public byte Yaw, Pitch;
        public byte[] Metadata;

        public AddMobPacket(Entity mob) {
            MessageID = RakPacketType.AddMob;
            Metadata = new byte[] { };
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(EID);
            encoder.Encode(Type);
            encoder.Encode(X);
            encoder.Encode(Y);
            encoder.Encode(Z);
            encoder.Encode(Yaw);
            encoder.Encode(Pitch);

            encoder.Encode((byte)0); // Metadata length
            encoder.Encode(Metadata);

            return encoder.Get();
        }
    }
}
