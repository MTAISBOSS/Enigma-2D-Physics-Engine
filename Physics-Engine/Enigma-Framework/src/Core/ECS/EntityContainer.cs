using Enigma_Framework.Core.DependencyInjection;

namespace Enigma_Framework.Core.ECS;

public class EntityContainer : IService
{
    private readonly List<Entity> entities = new();
    public IReadOnlyList<Entity> GetEntities => entities;
    public EntityContainer()
    {
        ServiceLocator.Instance.Register(this);
    }

    ~EntityContainer()
    {
        ServiceLocator.Instance.Unregister(this);
    }

    public void RegisterEntity(Entity entity)
    {
        if (!entities.Contains(entity)) entities.Add(entity);
    }

    public void UnregisterEntity(Entity entity)
    {
        if (entities.Contains(entity)) entities.Remove(entity);
    }

    public Entity? FindEntity(Guid id)
    {
        return entities.FirstOrDefault(e => e.Id == id);
    }
    public void Update()
    {
        foreach (var entity in entities) entity.Components.Update();
    }
}