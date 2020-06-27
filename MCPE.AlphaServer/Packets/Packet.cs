using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.Packets {
    public class Packet {
        public PacketType Type;

        public static Packet Parse(byte[] data) {
            try {
                var type = data[0];
                switch ((PacketType)type) {
                case PacketType.UnconnectedPing: return new UnconnectedPingPacket(data);
                case PacketType.OpenConnectionRequest1:
                case PacketType.OpenConnectionRequest2: return new OpenConnectionRequestPacket(data);
                default: {
                    if ((type & RakNetPacket.IS_CONNECTED) != 0)
                        return new RakNetPacket(data);
                    Console.WriteLine($"Unhandled packet type {type:X}.");
                    Console.WriteLine(Formatters.AsHex(data));
                    break;
                }
                }
            } catch (Exception ex) {
                Console.WriteLine($"Error while parsing a packet: {ex.Message}");
                Console.WriteLine($"StackTrace: \n{ex.StackTrace}");
                if (Debugger.IsAttached) throw;
            }
            return new Packet();
        }

        public T Get<T>() where T : Packet => this as T;

        public virtual byte[] Serialize() => new byte[] { (byte)Type };
        public override string ToString() => $"Packet {{ Type: {Type} }}";
    }
}
