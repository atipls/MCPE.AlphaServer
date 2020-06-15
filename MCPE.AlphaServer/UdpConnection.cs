using MCPE.AlphaServer.Utils;
using MCPE.AlphaServer.World;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MCPE.AlphaServer {
    public class UdpConnection {
        public IPEndPoint EndPoint;
        public DateTime LastUpdate;
        public RakTriad Sequence;
        public MinecraftPlayer Player;

        public bool Valid => (DateTime.Now - LastUpdate).TotalSeconds < 10;
        public UdpConnection(IPEndPoint endpoint) {
            EndPoint = endpoint;
            LastUpdate = DateTime.Now;
            Player = new MinecraftPlayer();
        }
    }
}
