using Physics_Engine.Core.Entity_Component_System;
using Physics_Engine.Core.Log_System;
using Physics_Engine.Core.Physics_2D;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Core.Transform;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision;

public abstract class Collider : Component
{
    private Transform.Transform Transform => Entity.Transform;
    public PhysicMaterial Material;
    public ShapeArea ShapeArea { get; set; }

    public bool IsTransformUpdateRequired = true;
    public AABBCollision AABBCollision { get; set; }
    public bool IsAabbCollisionUpdateRequired = true;
    public bool IsTrigger = false;
    public Vector2 Position
    {
        get => Transform.Position;
        set
        {
            Transform.Position = value;
            InvalidateCaches();
        }
    }
    public float Rotation
    {
        get => Transform.Rotation;
        set
        {
            Transform.Rotation = value;
            InvalidateCaches();

        }
    }
    public abstract bool Intersects(Collider other, out CollisionInfo collisionInfo);
    public abstract AABBCollision GetAABB();
    public abstract float CalculateRotationalInertia(float mass);
    public override bool IsAbleToDuplicate() => true;
    public void InvalidateCaches()
    {
        IsTransformUpdateRequired = true;
        IsAabbCollisionUpdateRequired = true;
    }
    public override void Start()
    {
        Material = new PhysicMaterial(0, 1, 0.6f, 1);
    }

    public void OnTriggerEnter(Collider other)
    {
        Logger.Log($"{Entity.Name} has collision with {other.Entity.Name}");
    }
}