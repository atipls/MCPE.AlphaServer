using MCPE.AlphaServer.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MCPE.AlphaServer.Packets {
    public class LoginRequestPacket : RakPacket {
        public string Username;
        public int Protocol1;
        public int Protocol2;
        public uint ClientID;
        public string RealmsData;
        public LoginRequestPacket(ref RakDecoder decoder) {
            Username = decoder.String();
            Protocol1 = decoder.Int();
            Protocol2 = decoder.Int();
            ClientID = decoder.Int().Unsigned();
            RealmsData = decoder.String();
        }

        public override string ToString() => $"LoginRequest {{ Username: {Username}, Protocol: {Protocol1}.{Protocol2}, ClientID: {ClientID:X}, RealmsData: '{RealmsData}' }}";
    }
}
