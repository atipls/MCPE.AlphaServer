using MCPE.AlphaServer.Utils;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPE.AlphaServer.Game;

public enum EntityDataType {
    Byte = 0,
    Short = 1,
    Int = 2,
    Float = 3,
    String = 4,
    ItemInstance = 5,
    Pos = 6
}

public class EntityData {
    struct EntityDataHolder {
        public EntityDataType Type;
        public bool IsDirty;
        public object Value;
    };

    private readonly Dictionary<int, EntityDataHolder> DefinedData = new();


    public void Define(int id, EntityDataType dataType) {
        DefinedData[id] = new EntityDataHolder {
            Type = dataType,
            IsDirty = false,
            Value = null
        };
    }

    public void Set(int id, object value) {
        if (!DefinedData.TryGetValue(id, out var holder))
            throw new Exception("Undefined data id");

        holder.Value = value;
        holder.IsDirty = true;
    }

    public T Get<T>(int id) {
        if (!DefinedData.TryGetValue(id, out var holder))
            throw new Exception("Undefined data id");

        return (T)holder.Value;
    }

    public void Decode(ref DataReader reader) {

    }

    public void Encode(ref DataWriter writer) {
        foreach (var (id, holder) in DefinedData) {
            if (!holder.IsDirty) continue;

            writer.Byte((byte)(((int)holder.Type << 5) | id));

            switch (holder.Type) {
                case EntityDataType.Byte:
                    writer.Byte((byte)holder.Value);
                    break;
                case EntityDataType.Short:
                    writer.Short(BinaryPrimitives.ReverseEndianness((short)holder.Value));
                    break;
                case EntityDataType.Int:
                    writer.Int(BinaryPrimitives.ReverseEndianness((int)holder.Value));
                    break;
                case EntityDataType.Float:
                    writer.UInt(BinaryPrimitives.ReverseEndianness(BitConverter.SingleToUInt32Bits((float)holder.Value)));
                    break;
                case EntityDataType.String:
                    var stringValue = holder.Value.ToString();
                    writer.UShort((ushort)stringValue.Length);
                    writer.RawData(Encoding.UTF8.GetBytes(stringValue));
                    break;
                case EntityDataType.ItemInstance:
                    var itemInstance = (ItemInstance)holder.Value;
                    writer.UShort(BinaryPrimitives.ReverseEndianness((ushort)itemInstance.ItemID));
                    writer.Byte(itemInstance.Count);
                    writer.UShort(BinaryPrimitives.ReverseEndianness((ushort)itemInstance.AuxValue));
                    break;
                case EntityDataType.Pos:
                    throw new NotImplementedException();
                default:
                    break;
            }
        }

        writer.Byte(0x7F);
    }

}
