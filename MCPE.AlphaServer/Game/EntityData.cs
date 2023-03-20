using MCPE.AlphaServer.Utils;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

public enum EntityDataKey {
    Flags = 0,
    Air = 1,

    IsSleeping = 16,
    SleepPosition = 17
}

public class EntityData {
    class EntityDataHolder {
        public EntityDataType Type;
        public bool IsDirty;
        public object Value;
    };

    private readonly Dictionary<EntityDataKey, EntityDataHolder> DefinedData = new();

    public void Define(EntityDataKey id, EntityDataType dataType) {
        DefinedData[id] = new EntityDataHolder {
            Type = dataType,
            IsDirty = false,
            Value = null
        };
    }

    public void Set(EntityDataKey id, object value) {
        if (!DefinedData.TryGetValue(id, out var holder))
            throw new Exception("Undefined data id");

        holder.Value = value;
        holder.IsDirty = true;
    }

    public T Get<T>(EntityDataKey id) {
        if (!DefinedData.TryGetValue(id, out var holder))
            throw new Exception("Undefined data id");

        return (T)holder.Value;
    }


    public void Decode(ref DataReader reader) {
        while (true) {
            var dataType = reader.Byte();
            if (dataType == 0x7F) break;

            var type = (EntityDataType)(dataType >> 5);
            var id = (EntityDataKey)(dataType & 0x1F);

            if (!DefinedData.TryGetValue(id, out var holder))
                throw new Exception("Undefined data id");

            switch (type) {
                case EntityDataType.Byte:
                    holder.Value = reader.Byte();
                    break;
                case EntityDataType.Short:
                    holder.Value = BinaryPrimitives.ReverseEndianness(reader.Short());
                    break;
                case EntityDataType.Int:
                    holder.Value = BinaryPrimitives.ReverseEndianness(reader.Int());
                    break;
                case EntityDataType.Float:
                    holder.Value = BitConverter.UInt32BitsToSingle(BinaryPrimitives.ReverseEndianness(reader.UInt()));
                    break;
                case EntityDataType.String:
                    holder.Value = reader.String();
                    break;
                case EntityDataType.ItemInstance:
                    holder.Value = new ItemInstance {
                        ItemID = BinaryPrimitives.ReverseEndianness(reader.UShort()),
                        Count = reader.Byte(),
                        AuxValue = BinaryPrimitives.ReverseEndianness(reader.UShort())
                    };
                    break;
                case EntityDataType.Pos:
                    var x = BinaryPrimitives.ReverseEndianness(reader.Int());
                    var y = BinaryPrimitives.ReverseEndianness(reader.Int());
                    var z = BinaryPrimitives.ReverseEndianness(reader.Int());
                    holder.Value = new Vector3(x, y, z);
                    break;
            }
        }
    }

    public void Encode(ref DataWriter writer) {
        foreach (var (id, holder) in DefinedData) {
            if (!holder.IsDirty) continue;

            writer.Byte((byte)(((int)holder.Type << 5) | (int)id));

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
                    var stringValue = holder.Value.ToString()!;
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

                default:
                    break;
            }
        }

        writer.Byte(0x7F);
    }
}
