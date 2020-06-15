using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public enum PacketType : byte {
        UnconnectedPing = 0x01,
        UnconnectedPong = 0x1C,

        OpenConnectionRequest1 = 0x05,
        OpenConnectionReply1 = 0x06,
        OpenConnectionRequest2 = 0x07,
        OpenConnectionReply2 = 0x08,

        RakNetPacket = 0xFF,
    }
}
