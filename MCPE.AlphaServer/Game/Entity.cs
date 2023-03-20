using System.Collections.Generic;

namespace MCPE.AlphaServer.Game;

public class Entity {
    private static int LastEntityID = 1;

    public int EntityID;
    
    public EntityData EntityData;

    public Entity() {
        EntityID = LastEntityID++;
        EntityData = new EntityData();

        Define(EntityDataKey.Flags, EntityDataType.Byte);
        Define(EntityDataKey.Air, EntityDataType.Short);
    }

    public void Define(EntityDataKey id, EntityDataType dataType) => EntityData.Define(id, dataType);
    public void Set(EntityDataKey id, object value) => EntityData.Set(id, value);
    public T Get<T>(EntityDataKey id) => EntityData.Get<T>(id);
}
