using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class RequestChunkPacket : RakPacket {
        public int X, Z;

        public RequestChunkPacket(ref RakDecoder decoder) {
            X = decoder.Int();
            Z = decoder.Int();
        }

        public override string ToString() => $"RequestChunk {{ X: {X}, Z: {Z} }}";
    }
}