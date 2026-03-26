namespace Physics_Engine.Core.Entity_Component_System;

public abstract class Component
{
    public Entity Entity { get; internal set; }
    public virtual void Start(){}
    public virtual void Update(){}
    public virtual bool IsAbleToDuplicate() => true;
}