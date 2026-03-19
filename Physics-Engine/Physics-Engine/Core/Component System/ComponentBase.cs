using Physics_Engine.Core.Physics_2D;

namespace Physics_Engine.Core.Component_System;

public abstract class ComponentBase : IComponent
{
    public PhysicsObject Owner { get; internal set; }
    public virtual bool IsAbleToDuplicate() => true;
}