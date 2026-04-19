using Enigma_Framework.Core.ECS;
using Enigma_Framework.Core.LogSystem;
using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Core.Rigidbody;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Collision;

public abstract class Collider : Component
{
    public bool IsAabbCollisionUpdateRequired = true;

    public bool IsTransformUpdateRequired = true;
    public bool IsTrigger { get; set; }
    public PhysicMaterial Material;
    private Enigma_Framework.Core.Transform.Transform Transform => Entity.Transform;
    public ShapeArea ShapeArea { get; set; }
    public AABBCollision AABBCollision { get; set; }

    public Vector2 Position
    {
        get => Transform.WorldPosition;
        set
        {
            Transform.WorldPosition = value;
            InvalidateCaches();
        }
    }

    public float Rotation
    {
        get => Transform.WorldRotation;
        set
        {
            Transform.WorldRotation = value;
            InvalidateCaches();
        }
    }

    public abstract bool Intersects(Collider other, out CollisionInfo collisionInfo);
    public abstract AABBCollision GetAABB();
    public abstract float CalculateRotationalInertia(float mass);

    public override bool IsAbleToDuplicate()
    {
        return true;
    }

    public void InvalidateCaches()
    {
        IsTransformUpdateRequired = true;
        IsAabbCollisionUpdateRequired = true;
    }

    public override void Start()
    {
        Material = new PhysicMaterial(1, 0.6f, 1);
    }

    public void OnTriggerEnter(Collider other)
    {
        Logger.Log($"{Entity.Name} has collision with {other.Entity.Name}");
    }
}