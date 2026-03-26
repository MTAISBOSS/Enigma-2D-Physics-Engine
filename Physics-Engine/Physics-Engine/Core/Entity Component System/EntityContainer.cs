using System.Collections.Generic;
using Physics_Engine.Core.Service_Locator;

namespace Physics_Engine.Core.Entity_Component_System;

public class EntityContainer : IService
{
    private readonly List<Entity> entities = new List<Entity>();

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
        if (!entities.Contains(entity))
        {
            entities.Add(entity);
        }
    }

    public void UnregisterEntity(Entity entity)
    {
        if (entities.Contains(entity))
        {
            entities.Remove(entity);
        }
    }

    public void Update()
    {
        foreach (var entity in entities)
        {
            entity.Components.Update();
        }
    }
}