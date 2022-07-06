using System;
using System.Buffers.Binary;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;

namespace MCPE.AlphaServer.Utils;

public class DataReader {
    private readonly MemoryStream stream;

    public DataReader(byte[] data) => stream = new MemoryStream(data);
    public DataReader(Memory<byte> data) => stream = new MemoryStream(data.ToArray());

    public bool IsEof => stream.Position >= stream.Length;

    private Span<byte> Get<T>() where T : struct {
        Span<byte> span = new byte[Unsafe.SizeOf<T>()];
        if (stream.Read(span) != span.Length)
            throw new EndOfStreamException();
        return span;
    }

    public Memory<byte> Read(int length) {
        var memory = new byte[length];
        if (stream.Read(memory) != length)
            throw new EndOfStreamException();
        return memory;
    }

    public byte Byte() => (byte) stream.ReadByte();

    public short Short() => BinaryPrimitives.ReadInt16BigEndian(Get<short>());
    public ushort UShort() => BinaryPrimitives.ReadUInt16BigEndian(Get<short>());

    public int Int() => BinaryPrimitives.ReadInt32BigEndian(Get<int>());
    public uint UInt() => BinaryPrimitives.ReadUInt32BigEndian(Get<int>());

    public long Long() => BinaryPrimitives.ReadInt64BigEndian(Get<long>());
    public ulong ULong() => BinaryPrimitives.ReadUInt64BigEndian(Get<long>());

    public float Float() => BitConverter.ToSingle(Get<float>());
    public double Double() => BitConverter.ToDouble(Get<double>());

    public string String() {
        var length = UShort();
        Span<byte> bytes = stackalloc byte[length];
        if (stream.Read(bytes) != length)
            throw new EndOfStreamException();
        return Encoding.UTF8.GetString(bytes);
    }

    public int Triad() {
        return (stream.ReadByte() << 16) | (stream.ReadByte() << 8) | stream.ReadByte();
    }

    public IPEndPoint IPEndPoint() {
        var version = Byte() == 4 ? 4 : 16; // 16 bytes for ipv6
        Span<byte> buffer = stackalloc byte[version];
        if (stream.Read(buffer) != version)
            throw new EndOfStreamException();
        return new IPEndPoint(new IPAddress(buffer), UShort());
    }

    public void RakNetMagic() {
        stream.Seek(16, SeekOrigin.Current);
    }
}