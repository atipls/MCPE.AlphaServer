using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MCPE.AlphaServer.Game {
    public class Player : Entity {
        public uint ID;
        public string Username;
        public Vector3 Position;

        public Player(string username, uint id) {
            Username = username;
            ID = id;
        }
    }
}
