using Physics_Engine.Core.Physics_2D;

namespace Physics_Engine.Core.Component_System;

public abstract class Component : IComponent
{
    public Entity Entity { get; internal set; }
    public virtual void Start(){}
    public virtual void Update(){}
    public virtual bool IsAbleToDuplicate() => true;
}