using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Utils {
    public static class SignedHelpers {
        public static ushort Unsigned(this short x) => (ushort)x;
        public static uint Unsigned(this int x) => (uint)x;
        public static ulong Unsigned(this long x) => (ulong)x;

        public static sbyte Signed(this byte x) => (sbyte)x;
        public static long Signed(this ushort x) => (short)x;
        public static int Signed(this uint x) => (int)x;
        public static long Signed(this ulong x) => (long)x;
    }
}
