using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Utils {
    public class RakTimestamp {
        public ulong Value;
        public RakTimestamp(ulong val) => Value = val;

        public static implicit operator ulong(RakTimestamp ts) => ts.Value;
        public override string ToString() => $"{Value}ms";
    }
}
