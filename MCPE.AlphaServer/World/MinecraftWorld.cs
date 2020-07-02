using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCPE.AlphaServer.World {
    public class MinecraftWorld {
        public List<MinecraftEntity> Entities;
        public List<MinecraftPlayer> Players;
        public Dictionary<string, object> Metadata;

        public int LastEID; // Last Entity ID

        public MinecraftPlayer GetPlayerByName(string name) => Players.FirstOrDefault(P => P.Username == name);
    }
}
