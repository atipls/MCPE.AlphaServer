using MCPE.AlphaServer.Utils;
using NUnit.Framework;
using System;
using System.Net;

namespace MCPE.AlphaServer.Tests {
    public class RakEncoderTests {
        RakEncoder Encoder;
        [SetUp]
        public void Setup() {
            Encoder = new RakEncoder();
        }

        [Test]
        public void Trivial_BigEndian() {
            Encoder.Encode(0xAABBCCDD.Signed());
            Assert.AreEqual(Encoder.Get().Length, 4);
            Assert.AreEqual(Encoder.Get(), new byte[] { 0xAA, 0xBB, 0xCC, 0xDD });
        }

        [Test]
        public void Trivial_LittleEndian() {
            Encoder.LEncode(0xAABBCCDD.Signed());
            Assert.AreEqual(Encoder.Get().Length, 4);
            Assert.AreEqual(Encoder.Get(), new byte[] { 0xDD, 0xCC, 0xBB, 0xAA });
        }

        [Test]
        public void NonTrivial_String() {
            Encoder.Encode("test");
            Assert.AreEqual(Encoder.Get().Length, 6);
            Assert.AreEqual(Encoder.Get(), new byte[] { 0x00, 0x04, 0x74, 0x65, 0x73, 0x74 });
        }

        [Test]
        public void NonTrivial_Timestamp() {
            Encoder.Encode(new RakTimestamp(0xAABBCCDDEEFF1122));
            Assert.AreEqual(Encoder.Get().Length, 8);
            Assert.AreEqual(Encoder.Get(), new byte[] { 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x11, 0x22 });
        }

        [Test]
        public void NonTrivial_Address() {
            var bytes = new byte[16];
            new Random().NextBytes(bytes);
            var endpoint = new RakAddress(new IPEndPoint(new IPAddress(bytes), 0x4ABC));

            Encoder.Encode(endpoint);
            var encoded = Encoder.Get();

            Assert.AreEqual(encoded.Length, 19);
            Assert.AreEqual(encoded[0], 6); //Is IPv6
            Assert.AreEqual(encoded[17], 0x4A); //First part of port
        }

        // Simple packet: Unconnected Pong
        [Test]
        public void NonTrivial_SimpleFullPacket() {

            Encoder.Encode((byte)0x1C);
            Encoder.Encode(0x1122334455667788);
            Encoder.Encode(0xFEEFAABBCCDDEE99.Signed());
            Encoder.AddMagic();
            Encoder.Encode("MCCPP;Demo;Steve");

            Assert.AreEqual(Encoder.Get(), new byte[] { 0x1C, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0xFE, 0xEF, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0x99, 0x00, 0xFF, 0xFF, 0x00, 0xFE, 0xFE, 0xFE, 0xFE, 0xFD, 0xFD, 0xFD, 0xFD, 0x12, 0x34, 0x56, 0x78, 0x00, 0x10, 0x4D, 0x43, 0x43, 0x50, 0x50, 0x3B, 0x44, 0x65, 0x6D, 0x6F, 0x3B, 0x53, 0x74, 0x65, 0x76, 0x65 });
        }
    }
}