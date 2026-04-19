namespace Enigma_Framework.Core.ECS;

public struct EntityFactory
{
    public static T Create<T>(string name = "", string tag = "") where T : Entity, new()
    {
        var obj = (T)Activator.CreateInstance(typeof(T), name, tag)!;
        return obj;
    }
}