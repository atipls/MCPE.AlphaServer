using System;
using System.Text;
using System.Collections.Generic;
using MCPE.AlphaServer.Utils;
using System.Linq.Expressions;
using System.Linq;

namespace MCPE.AlphaServer.Packets {
    public class RakNetPacket : Packet {
        public byte Flags;
        public RakTriad SequenceNumber;
        public List<RakPacket> Enclosing;

        public const int IS_ACK = 1 << 6;
        public const int IS_NAK = 1 << 5;
        public const int IS_CONNECTED = 1 << 7;

        public bool IsACKorNAK => (Flags & (IS_ACK | IS_NAK)) != 0;

        public short Count;
        public bool MinEqualsMax;
        public RakTriad PacketMin;
        public RakTriad PacketMax;

        public RakNetPacket(byte[] data) {
            Type = PacketType.RakNetPacket;
            var decoder = new RakDecoder(data);

            Flags = decoder.Byte();
            if (IsACKorNAK) {
                Enclosing = new List<RakPacket>();
                Count = decoder.Short();
                MinEqualsMax = decoder.Byte() != 0;
                if (MinEqualsMax) {
                    PacketMin = decoder.Triad();
                    PacketMax = PacketMin;
                    return;
                }
                PacketMax = decoder.Triad();
            } else {
                SequenceNumber = decoder.Triad();
                Enclosing = new List<RakPacket>();
                while (!decoder.AtEnd) { Enclosing.Add(RakPacket.Parse(ref decoder)); }
            }
        }
        public RakNetPacket() { }

        public RakNetPacket CreateResponse() {
            return new RakNetPacket {
                Flags = IS_CONNECTED,
                SequenceNumber = SequenceNumber,
                Enclosing = new List<RakPacket>()
            };
        }

        public RakNetPacket CreateACK() {
            return new RakNetPacket {
                Flags = IS_CONNECTED | IS_ACK,
                PacketMin = SequenceNumber,
                MinEqualsMax = true,
                Count = 1 /*1 range.*/,
            };
        }

        public static RakNetPacket Create(RakTriad Sequence) {
            return new RakNetPacket {
                Flags = IS_CONNECTED,
                SequenceNumber = Sequence,
                Enclosing = new List<RakPacket>()
            };
        }

        public override string ToString() {
            if ((Flags & IS_ACK) != 0)
                return $"RakNetPacket ACK: {{ Count: {Count}, MinEqualsMax: {MinEqualsMax}, PacketMin: {PacketMin}, PacketMax: {PacketMax} }}";
            if ((Flags & IS_NAK) != 0)
                return $"RakNetPacket NAK: {{ Count: {Count}, MinEqualsMax: {MinEqualsMax}, PacketMin: {PacketMin}, PacketMax: {PacketMax} }}";
            return $"RakNetPacket {{ Flags: {Flags}, SeqNum: {SequenceNumber}, {Enclosing.Count} packets. }}";
        }
        public override byte[] Serialize() {
            var encoder = new RakEncoder();
            encoder.Encode(Flags);
            if (IsACKorNAK) {
                encoder.Encode(Count);
                encoder.Encode(MinEqualsMax);
                encoder.Encode(PacketMin);
                if (!MinEqualsMax)
                    encoder.Encode(PacketMax);
            } else {
                encoder.Encode(SequenceNumber);
                foreach (var enclosing in Enclosing)
                    encoder.Encode(RakPacket.Create(enclosing));
            }
            return encoder.Get();
        }
    }
}
