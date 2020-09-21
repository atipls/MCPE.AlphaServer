using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class MovePlayerPacket : RakPacket {
        public int ID;
        public float X, Y, Z;
        public float Pitch, Yaw, Roll;

        public Vector3 Position => new Vector3(X, Y, Z);

        public MovePlayerPacket(ref RakDecoder decoder) {
            ID = decoder.Int();
            X = decoder.Float();
            Y = decoder.Float();
            Z = decoder.Float();
            Yaw = decoder.Float();
            Pitch = decoder.Float();
            Roll = decoder.Float();
        }

        public MovePlayerPacket(Vector3 position, int id, Vector3 rotation) {
            ID = id;
            X = position.X;
            Y = position.Y;
            Z = position.Z;
            Pitch = rotation.X;
            Yaw = rotation.Y;
            Roll = rotation.Z;
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(ID);
            encoder.Encode(X);
            encoder.Encode(Y);
            encoder.Encode(Z);
            encoder.Encode(Pitch);
            encoder.Encode(Yaw);
            encoder.Encode(Roll);

            return encoder.Get();
        }

        public override string ToString() => $"MovePlayer {{ ID: {ID}, Pos: [{X}, {Y}, {Z}], Rot: [{Pitch}, {Yaw}, {Roll}] }}";
    }
}
