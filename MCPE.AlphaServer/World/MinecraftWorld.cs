using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.World {
    public class MinecraftWorld {
        public List<MinecraftEntity> Entities;
        public List<MinecraftPlayer> Players;
        public Dictionary<string, object> Metadata;

        public static MinecraftWorld Load(string directory) {
            //NamedBinaryTag.TaggedValue v = new NamedBinaryTag.TaggedValue();
            return null;
        }
    }
}
