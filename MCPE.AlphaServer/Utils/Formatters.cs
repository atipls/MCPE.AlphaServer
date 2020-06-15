using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Runtime.InteropServices;

namespace MCPE.AlphaServer.Utils {
    class Formatters {
        public static string AsHex(ReadOnlySpan<byte> data) {
            static byte[] ReplaceInvalid(byte[] characters)
                => characters.Select((c) => c < 31 || c >= 127 ? (byte)'.' : c).ToArray();

            var result = new StringBuilder();
            const int width = 16;
            for (var pos = 0; pos < data.Length;) {
                var line = data.Slice(pos, Math.Min(width, data.Length - pos)).ToArray();
                var hex = string.Join(" ", line.Select(v => v.ToString("X2", CultureInfo.InvariantCulture)));
                hex += new string(' ', width * 3 - 1 - hex.Length);
                var asCharacters = Encoding.ASCII.GetString(ReplaceInvalid(line));
                result.Append(FormattableString.Invariant($"{pos:X4} {hex} {asCharacters}\n"));
                pos += line.Length;
            }
            return result.ToString();
        }
    }
}
