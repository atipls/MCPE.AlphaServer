using MCPE.AlphaServer.Utils;
using NUnit.Framework;
using System;

namespace MCPE.AlphaServer.Tests {
    public class RakDecoderTests {
        RakDecoder Decoder;
        [SetUp]
        public void Setup() { }

        [Test]
        public void SimplePacketTest() {
            Decoder = new RakDecoder(new byte[] { 0x01, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF, 0x11, 0x22, 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 });

            Assert.AreEqual(Decoder.Byte(), 0x01);
            Assert.AreEqual(Decoder.Timestamp().ToString(), new RakTimestamp(0xAABBCCDDEEFF1122).ToString());
            Assert.DoesNotThrow(Decoder.Magic);
            Assert.AreEqual(Decoder.AtEnd, true);
            Assert.Throws<IndexOutOfRangeException>(() => { Decoder.Byte(); }, "Should be out of bounds.");
        }
    }
}