using MCPE.AlphaServer.Utils;
using MCPE.AlphaServer.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class AddPlayerPacket : RakPacket {
        public ulong ID;
        public string Username;
        public int EID;
        public float X, Y, Z;
        public byte Yaw, Pitch;
        public short ItemID, ItemAuxValue; // For active item (?)
        
        // Synced Entity Data
        public byte[] Metadata;

        public AddPlayerPacket(Player player) {
            MessageID = RakPacketType.AddPlayer;
            ID = player.CID;
            Username = player.Username;
            EID = player.EID;
            X = player.Position.X;
            Y = player.Position.Y;
            Z = player.Position.Z;
            Pitch = 0;
            Yaw = 0;
            Metadata = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
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

            encoder.Encode((byte)Metadata.Length); // Metadata length
            encoder.Encode(Metadata);

            return encoder.Get();
        }
    }
}
