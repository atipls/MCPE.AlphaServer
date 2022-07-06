using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class MessagePacket : RakPacket {
        public string Username;
        public string Message;

        public MessagePacket(ref RakDecoder decoder) {
            Username = decoder.String();
            Message = decoder.String();
        }

        public MessagePacket(string username, string message) {
            MessageID = RakPacketType.Message;
            Username = username;
            Message = message;
        }

        public override byte[] Serialize() {
            var encoder = new RakEncoder();

            encoder.Encode(Username);
            encoder.Encode(Message);

            return encoder.Get();
        }

        public override string ToString() => $"Message: {{ '{Username}':'{Message}' }}";
    }
}