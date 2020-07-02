using MCPE.AlphaServer.Utils;
using MCPE.AlphaServer.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class AddPlayerPacket : RakPacket {
        public long ID;
        public string Username;
        public int EID;
        public float X, Y, Z;
        public byte Yaw, Pitch;
        public short ItemID, ItemAuxValue; // For active item (?)
        public byte[] Metadata;

        public AddPlayerPacket(MinecraftPlayer player) {
            MessageID = RakPacketType.AddPlayer;
            ID = player.ID;
            Username = player.Username;
            EID = player.EID;
            X = player.Position.X;
            Y = player.Position.Y;
            Z = player.Position.Z;
            Pitch = 0;
            Yaw = 0;
            Metadata = new byte[] { };
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(ID);
            encoder.Encode(Username);
            encoder.Encode(EID);
            encoder.Encode(X);
            encoder.Encode(Y);
            encoder.Encode(Z);
            encoder.Encode(Yaw);
            encoder.Encode(Pitch);
            encoder.Encode(ItemID);
            encoder.Encode(ItemAuxValue);

            encoder.Encode((byte)0); // Metadata length
            encoder.Encode(Metadata);

            return encoder.Get();
        }
    }
}
