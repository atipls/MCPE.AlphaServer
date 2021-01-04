using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPE.AlphaServer.Game {
    // Tags:
    /*
    Byte: 0
    Short: 1
    Int: 2
    Float: 3
    String: 4
    ItemInstance: 5
    Pos: 6     
     */
    public enum TypeTag {
        Byte = 0,
        Short = 1,
        Int = 2,
        Float = 3,
        String = 4, // Length = Short, Data = Bytes * Length
        ItemInstance = 5, // Short, Unsigned Char, Short
        Pos = 6, // Int, Int, Int
    }

    public struct DataItem {
        public TypeTag Tag;
        public ushort Flags;
        public bool IsDirty;

        public long Numeric;
        public float Float;
        public string String;

        public ItemInstance ItemInstance;

        public int PosX;
        public int PosY;
        public int PosZ;
    }

    public class EntityData {
        public List<DataItem> Items = new List<DataItem>();

        public void Add(byte value) => Items.Add(new DataItem { Tag = TypeTag.Byte, Numeric = value, IsDirty = true });
        public void Add(short value) => Items.Add(new DataItem { Tag = TypeTag.Short, Numeric = value, IsDirty = true });
        public void Add(int value) => Items.Add(new DataItem { Tag = TypeTag.Int, Numeric = value, IsDirty = true });
        public void Add(float value) => Items.Add(new DataItem { Tag = TypeTag.Float, Float = value, IsDirty = true });
        public void Add(string value) => Items.Add(new DataItem { Tag = TypeTag.String, String = value, IsDirty = true });
    }

    public struct SynchedEntityData {

    }
}
