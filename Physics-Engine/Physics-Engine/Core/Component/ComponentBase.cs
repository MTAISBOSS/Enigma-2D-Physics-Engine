using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Physics_Engine.Core;

namespace Physics_Engine.Core;

public abstract class ComponentBase : Component
{
    public PhysicsObject Owner { get; internal set; }
    public virtual bool IsAbleToBeCloned() => true;
}