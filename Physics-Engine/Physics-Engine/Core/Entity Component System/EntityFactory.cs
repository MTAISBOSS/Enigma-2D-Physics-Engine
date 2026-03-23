using System;

namespace Physics_Engine.Core.Entity_Component_System;

public struct EntityFactory
{
    public static T Create<T>(string name = "", string tag = "") where T : Entity, new()
    {
        var obj = (T)Activator.CreateInstance(typeof(T), name, tag)!;
        return obj;
    }
}