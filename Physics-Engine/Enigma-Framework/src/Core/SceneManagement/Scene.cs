using Enigma_Framework.Core.ECS;

namespace Enigma_Framework.Core.SceneManagement;

public class Scene
{
    public int Index;
    public string Name { get; set; }
    public EntityContainer Entities { get; } = new();

    public Scene(string name)
    {
        Name = name;
    }
    public void Update()
    {
        Entities.Update();
    }

    public Entity CreateEntity(string name)
    {
        var entity = new Entity(name);
        Entities.RegisterEntity(entity);
        return entity;
    }
}