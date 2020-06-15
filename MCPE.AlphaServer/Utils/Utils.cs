using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;
using System.Net.Sockets;

namespace MCPE.AlphaServer.Utils {
    public static class Utils {
        public static T[] Atleast<T>(this T[] arr, int size, T fill) {
            var final = arr.ToList();
            if (final.Count < size)
                final.AddRange(Enumerable.Repeat(fill, size - final.Count));
            return final.ToArray();
        }
        public static T[] ReverseIf<T>(this T[] arr, bool cond) {
            if (cond)
                return arr.Reverse().ToArray();
            return arr;
        }

        public static unsafe int ToIntBits(float value) => *(int*)&value;
        public static unsafe float ToFloatBits(int value) => *(float*)&value;

        public static unsafe long ToLongBits(double value) => *(long*)&value;
        public static unsafe double ToDoubleBits(long value) => *(double*)&value;
    }
}
