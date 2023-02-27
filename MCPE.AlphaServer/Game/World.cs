using System.IO;

namespace MCPE.AlphaServer.Game;

using MCPE.AlphaServer.NBT;

public class World
{
    private Chunk[,] _chunks;
    private NbtFile _levelDat;
    private NbtFile _entitiesDat;
    
    public Chunk this[int x, int z] => _chunks[x, z];
    
    public static World From(string folder)
    {
        NbtFile.BigEndianByDefault = false;
        
        var world = new World()
        {
            _chunks = new Chunk[16, 16],
            _levelDat = new NbtFile(),
            _entitiesDat = new NbtFile()
        };
        
        world._levelDat.LoadFromFileWithOffset(Path.Combine(folder, "level.dat"), 8);
        world._entitiesDat.LoadFromFileWithOffset(Path.Combine(folder, "entities.dat"), 12);
        
        using var chunksDat = File.OpenRead(Path.Combine(folder, "chunks.dat"));
        using var chunkReader = new BinaryReader(chunksDat);
        
        var chunkMetadata = Chunk.ReadMetadata(chunkReader);

        for (var xz = 0; xz < 16 * 16; xz++)
        {
            var x = xz % 16;
            var z = xz / 16;

            var offset = chunkMetadata[x, z];
            if (offset == 0)
                continue;

            chunksDat.Seek(offset, SeekOrigin.Begin);
            world._chunks[x, z] = Chunk.From(chunkReader);
        }

        return world;
    }
}