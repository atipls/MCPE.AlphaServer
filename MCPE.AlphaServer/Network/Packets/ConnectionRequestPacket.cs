using System;
using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.Packets {
    public class ConnectionRequestPacket : RakPacket {
        public ulong ClientGuid;
        public RakTimestamp TimeSinceStart;
        public byte UseEncryption;

        public ConnectionRequestPacket(ref RakDecoder decoder) {
            ClientGuid = decoder.Long().Unsigned();
            TimeSinceStart = decoder.Timestamp();
            UseEncryption = decoder.Byte();
        }

        public override string ToString() =>
            $"ConnectionRequest {{ ClientID: {ClientGuid:X}, TimeSinceStart: {TimeSinceStart}, Unknown: {UseEncryption} }}";
    }
}