using System.Collections.Generic;
using Physics_Engine.Core.Service_Locator;

namespace Physics_Engine.Core.Physics_2D;

public class EntityContainer : IService
{
    private readonly List<Entity> _entities = new List<Entity>();

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
        if (!_entities.Contains(entity))
        {
            _entities.Add(entity);
        }
    }

    public void UnregisterEntity(Entity entity)
    {
        if (_entities.Contains(entity))
        {
            _entities.Remove(entity);
        }
    }
}