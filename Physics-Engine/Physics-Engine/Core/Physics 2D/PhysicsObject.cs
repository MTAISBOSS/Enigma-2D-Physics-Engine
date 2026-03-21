using Physics_Engine.Core.Component_System;
using Physics_Engine.Core.Service_Locator;
using Physics_Engine.Core.Transform;

namespace Physics_Engine.Core.Physics_2D;

public class PhysicsObject
{
    public string Name { get; }
    public string Tag { get; }
    private Transform2D _transform;
    public Transform2D Transform
    {
        get
        {
            if (_transform == null)
            {
                _transform ??= new Transform2D();
                Components.Add(_transform);
            }
            return _transform;
        }
        set { _transform = value; }
    }
    private readonly ComponentContainer _components;
    public PhysicsObject(string name = "", string tag = "")
    {
        Name = name;
        Tag = tag;
        _components = new ComponentContainer(this);
        var physicsSystem = ServiceLocator.Instance.Get<PhysicsObjectContainer>();
        physicsSystem.RegisterObject(this);
    }
    public ComponentContainer Components => _components;
}