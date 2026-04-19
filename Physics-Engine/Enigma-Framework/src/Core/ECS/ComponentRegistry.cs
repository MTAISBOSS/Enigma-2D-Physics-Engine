using System.Reflection;
using System.Text.Json;
using Enigma_Framework.Core.LogSystem;

namespace Enigma_Framework.Core.ECS;

public static class ComponentRegistry
{
    private static readonly Dictionary<string,IComponentLoader> Loaders = new();
    
    public static void Initialize()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<ComponentLoaderAttribute>();

            if (attribute == null)
            {
                continue;
            }

            if (!typeof(IComponentLoader).IsAssignableFrom(type))
            {
                continue;
            }

            var loader = (IComponentLoader)Activator.CreateInstance(type)!;
            Loaders[attribute.Name] = loader;
        }
    }
    public static void Create(string name, Entity entity, JsonElement data)
    {
        if (Loaders.TryGetValue(name, out IComponentLoader loader))
        {
            loader.Load(entity, data);
        }
        else
        {
            Logger.LogWarning($"Unknown component type:{name}");
        }
    }
}