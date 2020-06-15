using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Net.Sockets;

namespace MCPE.AlphaServer.Utils {
    public class RakEncoder {
        public static readonly byte[] RakNetMagic = new byte[] { 0x00, 0xFF, 0xFF, 0x00, 0xFE, 0xFE, 0xFE, 0xFE, 0xFD, 0xFD, 0xFD, 0xFD, 0x12, 0x34, 0x56, 0x78 };
        private List<byte> Data { get; set; } = new List<byte>();

        public void Encode(bool val) => Data.Add(val ? (byte)0x01 : (byte)0x00);
        public void Encode(byte val) => Data.Add(val);

        public void Encode(short val) => Data.AddRange(BitConverter.GetBytes(val).Reverse());
        public void Encode(int val) => Data.AddRange(BitConverter.GetBytes(val).Reverse());
        public void Encode(long val) => Data.AddRange(BitConverter.GetBytes(val).Reverse());
        public void Encode(float val) => Data.AddRange(BitConverter.GetBytes(val).Reverse());
        public void Encode(double val) => Data.AddRange(BitConverter.GetBytes(val).Reverse());

        public void LEncode(short val) => Data.AddRange(BitConverter.GetBytes(val));
        public void LEncode(int val) => Data.AddRange(BitConverter.GetBytes(val));
        public void LEncode(long val) => Data.AddRange(BitConverter.GetBytes(val));
        public void LEncode(float val) => Data.AddRange(BitConverter.GetBytes(val));
        public void LEncode(double val) => Data.AddRange(BitConverter.GetBytes(val));
        public void LEncode(RakTriad val) {
            Encode(val.Value[2]);
            Encode(val.Value[1]);
            Encode(val.Value[0]);
        }
        public void Encode(byte[] val) => Data.AddRange(val);
        public void Encode(string val) {
            Encode((short)val.Length);
            Encode(Encoding.ASCII.GetBytes(val));
        }
        public void Encode(RakTimestamp val) => Encode(BitConverter.GetBytes(val).Reverse().ToArray());
        public void Encode(RakAddress val) {
            byte type = (byte)(val.Address.AddressFamily == AddressFamily.InterNetwork ? 0x04 : 0x06);
            Encode(type);
            Encode(val.Address.GetAddressBytes().Select(RakAddress.Swap).ToArray());
            Encode(val.Port);
        }
        public void Encode(RakTriad val) {
            Encode(val.Value[0]);
            Encode(val.Value[1]);
            Encode(val.Value[2]);
        }
        public void AddMagic() => Encode(RakNetMagic);

        public byte[] Get() => Data.ToArray();
    }
}
