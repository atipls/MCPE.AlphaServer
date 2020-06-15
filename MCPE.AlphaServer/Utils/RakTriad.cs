using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MCPE.AlphaServer.Utils {
    public class RakTriad {
        public static readonly RakTriad Zero = RakTriad.FromInt(0, false);

        public byte[] Value;
        public bool LittleEndian;

        public int IntValue => BitConverter.ToInt32(Value.Atleast<byte>(4, 0).ReverseIf(LittleEndian));
        public RakTriad(byte[] val, bool littleEndian) {
            Value = val;
            LittleEndian = littleEndian;
        }
        public static RakTriad FromInt(int value, bool littleEndian) {
            var data = new byte[] { (byte)(value & 0xFF), (byte)((value >> 8) & 0xFF), (byte)((value >> 16) & 0xFF) };
            return new RakTriad(data.ReverseIf(littleEndian), littleEndian);
        }
        public RakTriad Add(int val) => FromInt(IntValue + val, LittleEndian);
        public RakTriad Sub(int val) => FromInt(IntValue - val, LittleEndian);
        public static implicit operator byte[](RakTriad tri) => tri.Value.ReverseIf(tri.LittleEndian);
        public override string ToString() => $"{{ {(LittleEndian ? "LE" : "BE")}: {IntValue} }}";
    }
}
