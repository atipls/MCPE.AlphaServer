using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class LoginResponsePacket : RakPacket {
        public LoginStatus Status;
        public bool StatusOK => Status == LoginStatus.VersionsMatch;

        public static LoginResponsePacket FromRequest(LoginRequestPacket packet, LoginStatus status) {
            return new LoginResponsePacket {
                MessageFlags = packet.MessageFlags,
                ReliableNum = packet.ReliableNum.Add(1),
                OrderingIndex = packet.OrderingIndex,
                OrderingChannel = packet.OrderingChannel,
                MessageID = RakPacketType.LoginResponse,
                Status = status,
            };
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode((int)Status);

            return encoder.Get();
        }

        public enum LoginStatus : int {
            VersionsMatch,
            ClientOutdated,
            ServerOutdated,
        }
    }
}
