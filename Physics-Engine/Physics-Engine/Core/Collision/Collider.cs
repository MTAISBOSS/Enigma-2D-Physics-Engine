using Physics_Engine.Core.Entity_Component_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Transform;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision;

public abstract class Collider : Component
{
    private Transform.Transform Transform => Entity.Transform;

    public Vector2 Position
    {
        get => Transform.Position;
        set => Transform.Position = value;
    }

    public abstract bool Intersects(Collider other, out CollisionInfo collisionInfo);

    public override bool IsAbleToDuplicate() => true;
}