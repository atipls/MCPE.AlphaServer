using MCPE.AlphaServer.Packets;
using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MCPE.AlphaServer.Game {
    public class World {
        public static World The;
        public Server Server => Server.The;

        public List<Entity> Entities = new List<Entity>();
        public List<UdpConnection> Players = new List<UdpConnection>();
        public Dictionary<string, object> Metadata = new Dictionary<string, object>();

        public int LastEID; // Last Entity ID

        public UdpConnection GetPlayerByName(string name) => Players.FirstOrDefault(P => P.Player?.Username == name);
        public async Task AddPlayer(UdpConnection player) {
            player.Player.EID = LastEID++;
            foreach (var P in Players) {
                if (P == player)
                    continue;
                await Server.Send(player, new AddPlayerPacket(P.Player));
                await Server.Send(P, new AddPlayerPacket(player.Player));
            }
            Players.Add(player);
        }

        public async Task MovePlayer(UdpConnection player, Vector3 position, float pitch, float yaw) {
            player.Player.Position = position;
            
            // Test
            await Server.Send(player, new MovePlayerPacket(position, player.Player.EID, new Vector3(pitch, yaw + 1f, 0f)));
            
            
            foreach (var P in Players) {
                if (P == player)
                    continue;
                await Server.Send(P, new MovePlayerPacket(position, P.Player.EID, new Vector3(pitch, yaw, 0f)));
            }
        }
    }
}
