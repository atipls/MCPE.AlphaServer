#if false
namespace MCPE.AlphaServer.Game {
    public class World {
        public List<Entity> Entities = new List<Entity>();
        public List<Player> Players = new List<RakNetConnection>();

        public int LastEID; // Last Entity ID

        public RakNetConnection GetPlayerByName(string name) => Players.FirstOrDefault(P => P.Player?.Username == name);

        public async Task AddPlayer(RakNetConnection toAdd) {
            var newPlayer = Server.Clients[toAdd.EndPoint];
            foreach (var P in Players) {
                await Server.Send(P, new AddPlayerPacket(newPlayer.Player));
            }

            Players.Add(newPlayer);

        }


        public async Task MovePlayer(UdpConnection toMove, Vector3 position, float pitch, float yaw) {
            var player = Server.Clients[toMove.EndPoint];

            player.Player.Position = position;

            foreach (var P in Players) {
                if (P.Player.CID == player.Player.CID)
                    continue;
                await Server.Send(P, new MovePlayerPacket(position, player.Player.EID, new Vector3(pitch, yaw, 0f)));
            }
        }
    }
}
#endif