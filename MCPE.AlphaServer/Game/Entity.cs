using System.Collections.Generic;

namespace MCPE.AlphaServer.Game;

public class Entity {
    private static int LastEntityID = 1;

    public int EntityID;
    public EntityData EntityData;

    public const int SYNC_FLAGS = 0;
    public const int SYNC_AIR = 1;

    public Entity() {
        EntityID = LastEntityID++;
        EntityData = new EntityData();

        Define(SYNC_FLAGS, EntityDataType.Byte);
        Define(SYNC_AIR, EntityDataType.Short);
    }

    public void Define(int id, EntityDataType dataType) => EntityData.Define(id, dataType);
    public void Set(int id, object value) => EntityData.Set(id, value);
    public T Get<T>(int id) => EntityData.Get<T>(id);
}