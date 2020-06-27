using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MCPE.AlphaServer.World {
    public class MinecraftPlayer : MinecraftEntity {
        public uint ID;
        public string Username;
        public Vector3 Position;

        public MinecraftPlayer(string username, uint id) {
            Username = username;
            ID = id;
        }
    }
}
