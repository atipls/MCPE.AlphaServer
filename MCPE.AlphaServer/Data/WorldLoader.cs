using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MCPE.AlphaServer.NBT;

namespace MCPE.AlphaServer.Data;

public struct Block {
    public ushort ID;
    public byte Meta;
    public byte SkyLight;
    public byte BlockLight;
}

public struct Chunk {
    public uint Offset;

    public byte[,,] Blocks;

    public byte[,,] Meta;
    public byte[,,] SkyLight;
    public byte[,,] BlockLight;

    public Chunk(uint offset) {
        Offset = offset;
        Blocks = new byte[128, 16, 16];
        Meta = new byte[128, 16, 16];
        SkyLight = new byte[128, 16, 16];
        BlockLight = new byte[128, 16, 16];
    }

    public Block this[int x, int y, int z] {
        get => new() {
            ID = Blocks[y, z, x],
            Meta = Meta[y, z, x],
            SkyLight = SkyLight[y, z, x],
            BlockLight = BlockLight[y, z, x]
        };
    }
}

public class WorldManager {
    private readonly string BasePath;
        
    public Chunk[,] Chunks;

    public WorldManager(string basePath) {
        BasePath = basePath;
        if (Directory.Exists(BasePath))
            Load();
    }

    private void LoadChunks() {
        const int SECTOR_LENGTH = 0x1000;
        const int CHUNK_SIZE = 16 * 16 * 128;

        var reader = File.OpenRead(Path.Join(BasePath, "chunks.dat"));

        Chunks = new Chunk[16, 16];

        Span<byte> readerBuffer = stackalloc byte[4];
        for (int i = 0; i < SECTOR_LENGTH; i += 4) {
            reader.Read(readerBuffer);
            var data = BitConverter.ToUInt32(readerBuffer);

            if (data == 0)
                continue;

            // Every column is 32 chunks wide
            var x = (i >> 2) % 32;
            var z = (i >> 2) / 32;

            var sectorOffset = data >> 8;

            Chunks[x, z] = new Chunk(sectorOffset * CHUNK_SIZE);
        }

        // chunk size * (block data + block meta + block light + sky light)
        var chunkBuffer = new byte[82176];

        for (int cX = 0; cX < 16; cX++) {
            for (int cZ = 0; cZ < 16; cZ++) {
                ref var chunk = ref Chunks[cX, cZ];

                reader.Seek(chunk.Offset, SeekOrigin.Begin);

                reader.Read(chunkBuffer);

                Buffer.BlockCopy(chunkBuffer, 0, chunk.Blocks, 0, CHUNK_SIZE);
                for (int i = 0; i < CHUNK_SIZE; i++) {
                    // The block order is: YZX
                    var x = i % 16;
                    var y = i / (16 * 16);
                    var z = (i % 16) / 16;

                    var metaOffset = CHUNK_SIZE;
                    var skyLightOffset = metaOffset + 16384;
                    var blockLightOffset = skyLightOffset + 16384;

                    chunk.Meta[y, z, x] = (byte)(chunkBuffer[metaOffset + i / 2] & ((i & 1) == 0 ? 0x0F : 0xF0));
                    chunk.SkyLight[y, z, x] = (byte)(chunkBuffer[skyLightOffset + i / 2] & ((i & 1) == 0 ? 0x0F : 0xF0));
                    chunk.BlockLight[y, z, x] = (byte)(chunkBuffer[blockLightOffset + i / 2] & ((i & 1) == 0 ? 0x0F : 0xF0));
                }
            }
        }

        reader.Close();
    }

    private void LoadLevelInfo() {
        var stream = File.OpenRead(Path.Join(BasePath, "level.dat"));
        stream.Seek(8, SeekOrigin.Begin); // Skip header

        var reader = new NbtFile();
        reader.LoadFromStream(stream, NbtCompression.None);

    }

    private void LoadEntities() {
        var stream = File.OpenRead(Path.Join(BasePath, "entities.dat"));
        stream.Seek(12, SeekOrigin.Begin); // Skip header

        var reader = new NbtFile();
        reader.LoadFromStream(stream, NbtCompression.None);

    }

    public void Load() {
        NbtFile.BigEndianByDefault = false;
        
        LoadChunks();
        LoadLevelInfo();
        LoadEntities();
    }


    public Block this[int x, int y, int z] {
        get {
            var chunk = Chunks[x / 16, z / 16];
            return chunk[x % 16, y, z % 16];
        }
    }
}
