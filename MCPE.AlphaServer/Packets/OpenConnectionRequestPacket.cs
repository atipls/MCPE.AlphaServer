using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Linq;

namespace MCPE.AlphaServer.Packets {
    public class OpenConnectionRequestPacket : Packet {
        public byte ProtocolVersion;
        public RakAddress Address;
        public short MtuSize;
        public ulong ClientID;

        public OpenConnectionRequestPacket(byte[] data) {
            RakDecoder decoder = new RakDecoder(data);

            Type = (PacketType)decoder.Byte();
            decoder.Magic();
            switch (Type) {
            case PacketType.OpenConnectionRequest1:
                ProtocolVersion = decoder.Byte();
                break;
            case PacketType.OpenConnectionRequest2:
                Address = decoder.Address();
                MtuSize = decoder.Short();
                ClientID = decoder.Long().Unsigned();
                break;
            default: Debug.Assert(false, "Unreachable."); break;
            }

            // Null padding
            decoder.Raw(data.Length - decoder.Pos);
        }

        public override string ToString() {
            if (Type == PacketType.OpenConnectionRequest1)
                return $"OpenConnectionRequestPacket {{ Type: {Type}, ProtocolVersion: {ProtocolVersion:X} }}";
            return $"OpenConnectionRequestPacket {{ Type: {Type}, ServerAddress: {Address}, MtuSize: {MtuSize}, ClientID: {ClientID:X8} }}";
        }
    }
}
