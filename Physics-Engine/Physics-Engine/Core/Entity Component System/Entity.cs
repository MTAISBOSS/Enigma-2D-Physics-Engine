using Physics_Engine.Core.Service_Locator;

namespace Physics_Engine.Core.Entity_Component_System;

public class Entity
{
    public string Name { get; }
    public string Tag { get; }
    private Transform.Transform _transform;
    public Transform.Transform Transform
    {
        get
        {
            if (_transform == null)
            {
                _transform ??= new Transform.Transform();
                Components.Add(_transform);
            }
            return _transform;
        }
        set { _transform = value; }
    }
    private readonly ComponentContainer _components;
    public Entity(string name = "", string tag = "")
    {
        Name = name;
        Tag = tag;
        _components = new ComponentContainer(this);
        var physicsSystem = ServiceLocator.Instance.Get<EntityContainer>();
        physicsSystem.RegisterEntity(this);
    }

    ~Entity()
    {
        var physicsSystem = ServiceLocator.Instance.Get<EntityContainer>();
        physicsSystem.UnregisterEntity(this);
    }
    public ComponentContainer Components => _components;
}