using MCPE.AlphaServer.Packets;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCPE.AlphaServer.Game {
    public class World {
        public static World The;
        public Server Server => Server.The;

        public List<Entity> Entities;
        public List<UdpConnection> Players;
        public Dictionary<string, object> Metadata;

        public int LastEID; // Last Entity ID

        public UdpConnection GetPlayerByName(string name) => Players.FirstOrDefault(P => P.Player?.Username == name);
        public async void AddPlayer(UdpConnection player) {
            foreach (var P in Players) {
                await Server.Send(P, new AddPlayerPacket(P.Player));
            }
            player.Player.EID = LastEID++;
            Players.Add(player);
        }
    }
}
