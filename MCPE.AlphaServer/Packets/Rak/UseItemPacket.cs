using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class UseItemPacket : RakPacket {
        public int X, Y, Z;
        public int Face;
        public short Block;
        public byte Meta;
        public int ID;
        public float Fx, Fy, Fz;
        public float Px, Py, Pz;

        public UseItemPacket(ref RakDecoder decoder) {
            X = decoder.Int();
            Y = decoder.Int();
            Z = decoder.Int();

            Face = decoder.Int();
            Block = decoder.Short();
            Meta = decoder.Byte();
            ID = decoder.Int();

            Fx = decoder.Float();
            Fy = decoder.Float();
            Fz = decoder.Float();

            Px = decoder.Float();
            Py = decoder.Float();
            Pz = decoder.Float();
        }

        public override string ToString() => $"UseItem {{ Pos: [{X}, {Y}, {Z}], Face: {Face}, Block: {Block}, Meta: {Meta}, ID: {ID}, F: [{Fx}, {Fy}, {Fz}], P: [{Px}, {Py}, {Pz}] }}";
    }
}
