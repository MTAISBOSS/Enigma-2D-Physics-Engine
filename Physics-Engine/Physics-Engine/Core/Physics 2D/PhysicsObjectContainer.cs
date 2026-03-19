using System.Collections.Generic;
using Physics_Engine.Core.Service_Locator;

namespace Physics_Engine.Core.Physics_2D;

public class PhysicsObjectContainer : IService
{
    private readonly List<PhysicsObject> _physicsObjects = new List<PhysicsObject>();

    public PhysicsObjectContainer()
    {
        ServiceLocator.Instance.Register(this);
    }

    ~PhysicsObjectContainer()
    {
        ServiceLocator.Instance.Unregister(this);
    }

    public void RegisterObject(PhysicsObject physicsObject)
    {
        if (!_physicsObjects.Contains(physicsObject))
        {
            _physicsObjects.Add(physicsObject);
        }
    }

    public void UnregisterObject(PhysicsObject physicsObject)
    {
        if (_physicsObjects.Contains(physicsObject))
        {
            _physicsObjects.Remove(physicsObject);
        }
    }
}