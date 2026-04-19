using Enigma_Framework.Core.DependencyInjection;
using Enigma_Framework.Core.Physics2D;
using Enigma_Framework.Core.Rigidbody;

namespace Enigma_Framework.Core.Collision;

public class CircleCollider : Collider
{
    public CircleArea CircleArea { get; set; }

    public override bool Intersects(Collider other, out CollisionInfo collisionInfo)
    {
        collisionInfo = new CollisionInfo();
        return CollisionDetector.Intersect(this, other, out collisionInfo);
    }

    public override void Start()
    {
        CircleArea = new CircleArea(Entity.Transform.WorldScale.x);
        IsTransformUpdateRequired = true;
        PhysicsContext.RegisterCollider(this);
    }

    ~CircleCollider()
    {
        PhysicsContext.UnregisterCollider(this);
    }

    public override AABBCollision GetAABB()
    {
        if (!IsAabbCollisionUpdateRequired) return AABBCollision;
        var minX = Position.x - CircleArea.Radius;
        var minY = Position.y - CircleArea.Radius;
        var maxX = Position.x + CircleArea.Radius;
        var maxY = Position.y + CircleArea.Radius;

        IsAabbCollisionUpdateRequired = false;

        AABBCollision = new AABBCollision(minX, minY, maxX, maxY);
        return AABBCollision;
    }

    public override float CalculateRotationalInertia(float mass)
    {
        return 0.5f * mass * CircleArea.Radius * CircleArea.Radius;
    }
}