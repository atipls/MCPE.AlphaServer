using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class AnimatePacket : RakPacket {
        public byte Action;
        public int ID;

        public AnimatePacket(ref RakDecoder decoder) {
            Action = decoder.Byte();
            ID = decoder.Int();
        }

        public override string ToString() => $"Animate {{ Action: {Action}, ID: {ID} }}";
    }
}