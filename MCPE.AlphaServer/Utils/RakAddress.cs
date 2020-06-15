using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MCPE.AlphaServer.Utils {
    public class RakAddress {
        public IPAddress Address;
        public short Port;

        public RakAddress(IPEndPoint endpoint) {
            Address = endpoint.Address;
            Port = (short)endpoint.Port;
        }
        public RakAddress(IPAddress addr, short port) {
            Address = addr;
            Port = port;
        }
        public static byte Swap(byte x) => (byte)(x ^ 255);

        public static implicit operator IPEndPoint(RakAddress addr) => new IPEndPoint(addr.Address, addr.Port);
        public override string ToString() => $"{Address}:{Port}";
    }
}
