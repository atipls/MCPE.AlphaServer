using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Linq;

namespace MCPE.AlphaServer.Packets {
    public class OpenConnectionReplyPacket : Packet {
        public ulong ServerGuid;
        public bool UseEncryption;
        public short MtuSize;
        public RakAddress ClientAddress;

        public static OpenConnectionReplyPacket FromRequest(OpenConnectionRequestPacket packet,
            ulong serverguid, IPEndPoint client, bool useencryption = false, short mtusize = 1492) {
            return new OpenConnectionReplyPacket() {
                Type = packet.Type + 1,
                ServerGuid = serverguid,
                UseEncryption = useencryption,
                MtuSize = mtusize,
                ClientAddress = new RakAddress(client)
            };
        }

        public override byte[] Serialize() {
            RakEncoder encoder = new RakEncoder();
            encoder.Encode((byte)Type);
            encoder.AddMagic();

            switch (Type) {
            case PacketType.OpenConnectionReply1: {
                encoder.Encode(ServerGuid.Signed());
                encoder.Encode(UseEncryption);
                encoder.Encode(MtuSize);
                break;
            }
            case PacketType.OpenConnectionReply2: {
                encoder.Encode(ServerGuid.Signed());
                encoder.Encode(ClientAddress);
                encoder.Encode(MtuSize);
                encoder.Encode(UseEncryption);
                break;
            }
            default: Debug.Assert(false, "Unreachable."); break;
            }

            return encoder.Get();
        }
    }
}
