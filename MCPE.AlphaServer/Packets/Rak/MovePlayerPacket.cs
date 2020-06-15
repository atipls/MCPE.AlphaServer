using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class MovePlayerPacket : RakPacket {
        public int ID;
        public float X, Y, Z;
        public float Pitch, Yaw, Roll;
        public MovePlayerPacket(ref RakDecoder decoder) {
            ID = decoder.Int();
            X = decoder.Float();
            Y = decoder.Float();
            Z = decoder.Float();
            Yaw = decoder.Float();
            Pitch = decoder.Float();
            Roll = decoder.Float();
        }

        public override string ToString() => $"MovePlayer {{ ID: {ID}, Pos: [{X}, {Y}, {Z}], Rot: [{Pitch}, {Yaw}, {Roll}] }}";
    }
}
