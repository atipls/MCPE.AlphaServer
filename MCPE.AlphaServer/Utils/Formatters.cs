using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MCPE.AlphaServer.Utils;

internal class Formatters {
    public static string AsHex(ReadOnlySpan<byte> data) {
        static byte[] ReplaceInvalid(IEnumerable<byte> characters) {
            return characters.Select(c => c is < 31 or >= 127 ? (byte)'.' : c).ToArray();
        }

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
