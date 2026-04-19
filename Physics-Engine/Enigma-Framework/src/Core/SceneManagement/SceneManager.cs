using System.Text.Json;
using Enigma_Framework.Core.ECS;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Enigma_Framework.Core.SceneManagement;

public static class SceneManager
{
    public static Scene? ActiveScene { get; private set; }

    public static Scene CreateNewScene(string name)
    {
        ActiveScene = new Scene(name);
        return ActiveScene;
    }
    
    public static void SetActiveScene(Scene scene)
    {
        ActiveScene = scene;
    }

    public static void UnloadScene()
    {
        ActiveScene = null;
    }

    public static Scene Load(string path)
    {
        var json = File.ReadAllText(path);
        var data = JsonSerializer.Deserialize<SceneData>(json);
        var scene = new Scene(data?.Name);
        var entityMap = new Dictionary<Guid, Entity>();
        foreach (var entityData in data.Entities)
        {
            var entity = scene.CreateEntity(entityData.Name);
            entityMap[entityData.id] = entity;
        }

        foreach (var entityData in data.Entities)
        {
            var entity = entityMap[entityData.id];

            foreach (var component in entityData.Components)
            {
                ComponentRegistry.Create(component.Key, entity, component.Value);
            }
        }


        return scene;
    }
}