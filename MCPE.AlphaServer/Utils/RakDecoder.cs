using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net;

namespace MCPE.AlphaServer.Utils {
    public class RakDecoder {
        public int Pos = 0;
        private byte[] Data;
        public bool AtEnd => Pos >= Data.Length;

        public RakDecoder(byte[] data) => Data = data;

        public long Long() => ((long)Data[Pos++] << 56) | ((long)Data[Pos++] << 48) | ((long)Data[Pos++] << 40) | ((long)Data[Pos++] << 32) | ((long)Data[Pos++] << 24) | ((long)Data[Pos++] << 16) | ((long)Data[Pos++] << 8) | Data[Pos++];
        public long LLong() => Data[Pos++] | ((long)Data[Pos++] << 8) | ((long)Data[Pos++] << 16) | ((long)Data[Pos++] << 24) | ((long)Data[Pos++] << 32) | ((long)Data[Pos++] << 40) | ((long)Data[Pos++] << 48) | ((long)Data[Pos++] << 56);

        public int Int() => (Data[Pos++] << 24) | (Data[Pos++] << 16) | (Data[Pos++] << 8) | Data[Pos++];
        public int LInt() => Data[Pos++] | (Data[Pos++] << 8) | (Data[Pos++] << 16) | (Data[Pos++] << 24);

        public short Short() => (short)((Data[Pos++] << 8) | Data[Pos++]);
        public short LShort() => (short)(Data[Pos++] | (Data[Pos++] << 8));

        public float Float() => Utils.ToFloatBits(Int());
        public float LFloat() => Utils.ToFloatBits(LInt());

        public double Double() => Utils.ToDoubleBits(Long());
        public double LDouble() => Utils.ToDoubleBits(LLong());

        public byte Byte() => Data[Pos++];
        public byte[] From => Data[Pos..];
        public byte[] Raw(int start, int amnt) => Data[start..(start + amnt)];
        public byte[] Raw(int len) {
            var ret = Data[Pos..(Pos + len)];
            Pos += len;
            return ret;
        }

        public string String() => Encoding.ASCII.GetString(Raw(Short()));

        public RakTimestamp Timestamp() => new RakTimestamp(Long().Unsigned());
        public RakAddress Address() {
            var version = Byte() == 4 ? 4 : 16; // 16 bytes for ipv6
            var bytes = Raw(version).Select(RakAddress.Swap).ToArray();
            return new RakAddress(new IPAddress(bytes), Short());
        }

        public RakTriad Triad() => new RakTriad(Raw(3), false);
        public RakTriad LTriad() => new RakTriad(Raw(3), true);

        public void Magic() => Pos += 16;
    }
}
