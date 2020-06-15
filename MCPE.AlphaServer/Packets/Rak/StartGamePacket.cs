using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class StartGamePacket : RakPacket {
        public int LevelSeed;
        public int Unknown;
        public int Gamemode;
        public int EntityID;
        public float PosX;
        public float PosY;
        public float PosZ;

        public StartGamePacket(int relnum) {
            MessageFlags = IS_RELIABLE;
            ReliableNum = RakTriad.FromInt(relnum, false);
            MessageID = RakPacketType.StartGame;
            LevelSeed = 1;
            PosX = PosY = PosZ = 100.0f;
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(LevelSeed);
            encoder.Encode(Unknown);
            encoder.Encode(Gamemode);
            encoder.Encode(EntityID);
            encoder.Encode(PosX);
            encoder.Encode(PosY);
            encoder.Encode(PosZ);

            return encoder.Get();
        }
    }
}