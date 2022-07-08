using System;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using MCPE.AlphaServer.RakNet;

namespace MCPE.AlphaServer.Utils;

public class DataWriter {
    private readonly MemoryStream stream;

    public DataWriter() {
        stream = new MemoryStream();
    }

    public int Length => (int)stream.Length;

    public byte[] GetBytes() => stream.ToArray();
    public void RawData(byte[] data) => stream.Write(data);
    public void Byte(byte value) => stream.WriteByte(value);

    public void Short(short value) {
        Span<byte> span = stackalloc byte[2];
        BinaryPrimitives.WriteInt16BigEndian(span, value);
        stream.Write(span);
    }

    public void UShort(ushort value) {
        Span<byte> span = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(span, value);
        stream.Write(span);
    }

    public void Int(int value) {
        Span<byte> span = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(span, value);
        stream.Write(span);
    }

    public void UInt(uint value) {
        Span<byte> span = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(span, value);
        stream.Write(span);
    }

    public void Long(long value) {
        Span<byte> span = stackalloc byte[8];
        BinaryPrimitives.WriteInt64BigEndian(span, value);
        stream.Write(span);
    }

    public void ULong(ulong value) {
        Span<byte> span = stackalloc byte[8];
        BinaryPrimitives.WriteUInt64BigEndian(span, value);
        stream.Write(span);
    }

    public void Float(float value) {
        Span<byte> span = stackalloc byte[4];
        BinaryPrimitives.WriteSingleBigEndian(span, value);
        stream.Write(span);
    }

    public void Double(double value) {
        Span<byte> span = stackalloc byte[8];
        BinaryPrimitives.WriteDoubleBigEndian(span, value);
        stream.Write(span);
    }

    public void String(string value) {
        var bytes = Encoding.UTF8.GetBytes(value);
        UShort((ushort)bytes.Length);
        RawData(bytes);
    }

    public void Triad(int value) {
        Span<byte> span = stackalloc byte[3];
        span[2] = (byte)((value >> 16) & 0xFF);
        span[1] = (byte)((value >> 8) & 0xFF);
        span[0] = (byte)(value & 0xFF);
        stream.Write(span);
    }

    public void IPEndPoint(IPEndPoint value) {
        Byte((byte)(value.Address.AddressFamily == AddressFamily.InterNetwork ? 0x04 : 0x06));
        // RakNet does this so routers don't mess with the ips?
        RawData(value.Address.GetAddressBytes().Select(x => (byte)(x ^ 255)).ToArray());
        UShort((ushort)value.Port);
    }

    public void RakNetMagic() {
        RawData(UnconnectedPacket.RakNetMagic);
    }

    public void Vector3(Vector3 value) {
        Float(value.X);
        Float(value.Y);
        Float(value.Z);
    }
}