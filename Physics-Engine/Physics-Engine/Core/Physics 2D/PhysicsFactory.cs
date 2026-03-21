using System;

namespace Physics_Engine.Core.Physics_2D;

public struct PhysicsFactory
{
    public static T Create<T>(string name = "", string tag = "") where T : PhysicsObject, new()
    {
        var obj = (T)Activator.CreateInstance(typeof(T), name, tag)!;
        return obj;
    }
}