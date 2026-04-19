using Enigma_Framework.Core.Rigidbody;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Collision;

public readonly struct CollisionManifold
{
    public readonly Rigidbody2D BodyA => ColliderA.Entity.Components.Get<Rigidbody2D>();
    public readonly Rigidbody2D BodyB => ColliderB.Entity.Components.Get<Rigidbody2D>();
    public readonly Collider ColliderA;
    public readonly Collider ColliderB;
    public readonly CollisionInfo CollisionInfo;
    public readonly Vector2 Contact1;
    public readonly Vector2 Contact2;
    public readonly int ContactCount;

    public CollisionManifold(Collider colliderA, Collider colliderB, CollisionInfo collisionInfo, Vector2 contact1,
        Vector2 contact2, int contactCount)
    {
        CollisionInfo = collisionInfo;
        Contact1 = contact1;
        Contact2 = contact2;
        ContactCount = contactCount;
        ColliderA = colliderA;
        ColliderB = colliderB;
    }
}