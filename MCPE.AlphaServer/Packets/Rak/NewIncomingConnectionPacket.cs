using MCPE.AlphaServer.Utils;

namespace MCPE.AlphaServer.Packets {
    public class NewIncomingConnectionPacket : RakPacket {
        public RakAddress ServerAddress;
        public RakAddress[] InternalAddress;
        public RakTimestamp TimeSinceStart1;
        public RakTimestamp TimeSinceStart2;

        public NewIncomingConnectionPacket(ref RakDecoder decoder) {
            InternalAddress = new RakAddress[10];
            ServerAddress = decoder.Address();
            for (int i = 0; i < 10; i++)
                InternalAddress[i] = decoder.Address();
            TimeSinceStart1 = decoder.Timestamp();
            TimeSinceStart2 = decoder.Timestamp();
        }

        public override string ToString() => $"NewIncomingConnection {{ ServerAddress: {ServerAddress} }}";
    }
}