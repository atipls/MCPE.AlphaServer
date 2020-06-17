using MCPE.AlphaServer.Packets;
using MCPE.AlphaServer.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;
using Status = MCPE.AlphaServer.Packets.LoginResponsePacket.LoginStatus;
namespace MCPE.AlphaServer {
    class Server {
        public IPEndPoint ListenEndpoints { get; private set; }
        public UdpClient UdpServer { get; private set; }
        public DateTime StartTime { get; private set; }
        public Dictionary<IPEndPoint, UdpConnection> Clients { get; private set; }

        public ulong Guid { get; private set; } = 0x1122334455667788;
        public bool IsRunning = true;

        public Server(int port) : this(new IPEndPoint(IPAddress.Any, port)) { }
        public Server(IPEndPoint endpoints) {
            ListenEndpoints = endpoints;
            UdpServer = new UdpClient(endpoints);
            Clients = new Dictionary<IPEndPoint, UdpConnection>();
            StartTime = DateTime.Now;
            IsRunning = true;
        }

        public async Task Update() {
            var result = await UdpServer.ReceiveAsync();
            var parsed = Packet.Parse(result.Buffer);
            var endPoint = result.RemoteEndPoint;

            switch (parsed.Type) {
                case PacketType.UnconnectedPing: { await SendRaw(endPoint, UnconnectedPongPacket.FromPing(parsed.Get<UnconnectedPingPacket>(), Guid, "MCPE.AlphaServer")); break; }
                case PacketType.OpenConnectionRequest1: { await SendRaw(endPoint, OpenConnectionReplyPacket.FromRequest(parsed.Get<OpenConnectionRequestPacket>(), Guid, endPoint)); break; }
                case PacketType.OpenConnectionRequest2: {
                    var packet = parsed.Get<OpenConnectionRequestPacket>();
                    Clients.Add(endPoint, new UdpConnection(endPoint));
                    await SendRaw(endPoint, OpenConnectionReplyPacket.FromRequest(packet, Guid, endPoint));
                    break;
                }
                case PacketType.RakNetPacket: { await HandleRakNetPacket(parsed.Get<RakNetPacket>(), endPoint); break; }
                default: break;
            }
        }

        async Task HandleRakNetPacket(RakNetPacket rakPacket, IPEndPoint endpoint) {
            var Client = Clients[endpoint];
            Client.LastUpdate = DateTime.Now;
            if (!rakPacket.IsACKorNAK)
                Client.Sequence = rakPacket.SequenceNumber;

            foreach (var enclosing in rakPacket.Enclosing) {
                switch (enclosing.MessageID) {
                    case RakPacketType.ConnectedPong: {
                        Console.WriteLine("[<=] PONG!");
                        break;
                    }
                    case RakPacketType.ConnectedPing: { await Send(Client, ConnectedPongPacket.FromPing(enclosing.Get<ConnectedPingPacket>(), StartTime)); break; }
                    case RakPacketType.ConnectionRequest: { await Send(Client, ConnectionRequestAcceptedPacket.FromRequest(enclosing.Get<ConnectionRequestPacket>(), endpoint)); break; }
                    case RakPacketType.LoginRequest: {
                        var request = enclosing.Get<LoginRequestPacket>();
                        Client.Player = new World.MinecraftPlayer(request.Username);
                        var status = request.StatusFor(14);
                        var response = LoginResponsePacket.FromRequest(request, status);
                        await Send(
                            Client,
                            response,
                            response.StatusOK ? new StartGamePacket(request.ReliableNum.IntValue) : null
                        );
                        break;
                    }
                    case RakPacketType.NewIncomingConnection: {
                        Console.WriteLine($"[ +] {endpoint}");
                        break;
                    }
                    case RakPacketType.MovePlayer: {
                        var packet = enclosing.Get<MovePlayerPacket>();
                        await SendToEveryone(new MessagePacket("Server", $"{Client.Player.Username} moving at [{packet.X}, {packet.Y}, {packet.Z}]"));
                        break;
                    }
                    default:
                        break;
                }
            }

            if (!rakPacket.IsACKorNAK) {
                var ackPacket = rakPacket.CreateACK();
                var ackData = ackPacket.Serialize();
                await UdpServer.SendAsync(ackData, ackData.Length, endpoint);
            }
        }

        public async Task Send(UdpConnection Client, params RakPacket[] packets) {
            var rakPacket = RakNetPacket.Create(Client.Sequence);
            Client.Sequence = Client.Sequence.Add(1);
            rakPacket.Enclosing.AddRange(packets);
            var data = rakPacket.Serialize();
            await UdpServer.SendAsync(data, data.Length, Client.EndPoint);
        }

        public async Task SendRaw(IPEndPoint endPoint, Packet packet) {
            var data = packet.Serialize();
            await UdpServer.SendAsync(data, data.Length, endPoint);
        }

        public async Task SendToEveryone(params RakPacket[] packets) {
            foreach (var Client in Clients.Values) {
                await Send(Client, packets);
            }
        }

        public void ListenerThread() { while (true) { Task.Run(Update).GetAwaiter().GetResult(); } }
        public void ClientUpdaterThread() {
            while (true) {
                var disconnected = Clients.Where(x => !x.Value.Valid);
                foreach (var client in disconnected) {
                    //TODO: Events??
                    Console.WriteLine($"[ -] {client.Key}");
                    Clients.Remove(client.Key);
                }
                Thread.Sleep(100);
            }
        }
    }
}
