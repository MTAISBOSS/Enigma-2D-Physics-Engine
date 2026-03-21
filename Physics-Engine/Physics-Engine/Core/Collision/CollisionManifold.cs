using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision;

public struct CollisionManifold
{
    public readonly Rigidbody2D BodyA;
    public readonly Rigidbody2D BodyB;
    public readonly CollisionInfo CollisionInfo;
    public readonly Vector2 Contact1;
    public readonly Vector2 Contact2;
    public readonly int ContactCount;

    public CollisionManifold(Rigidbody2D bodyA, Rigidbody2D bodyB, CollisionInfo collisionInfo, Vector2 contact1, Vector2 contact2, int contactCount)
    {
        BodyA = bodyA;
        BodyB = bodyB;
        CollisionInfo = collisionInfo;
        Contact1 = contact1;
        Contact2 = contact2;
        ContactCount = contactCount;
    }
}