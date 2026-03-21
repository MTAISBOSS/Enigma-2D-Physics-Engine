using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision;

public static class CollisionResolver
{
    public static void Resolve(in CollisionManifold contact)
    {
        var bodyA = contact.BodyA;
        var bodyB = contact.BodyB;
        var collisionInfo = contact.CollisionInfo;

        Vector2 relativeVelocity = bodyB.Body.LinearVelocity - bodyA.Body.LinearVelocity;

        bool isNormalAndVelocityInSameDirection = Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal) > 0f;
        if (isNormalAndVelocityInSameDirection)
        {
            return;
        }

        float e = System.Math.Min(bodyA.Body.Restitution, bodyB.Body.Restitution);
        float j = -(1f + e) * Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal);
        j /= bodyA.Body.InverseMass + bodyB.Body.InverseMass;

        Vector2 impulse = j * collisionInfo.Normal;
        bodyA.Body.LinearVelocity -= impulse * bodyA.Body.InverseMass;
        bodyB.Body.LinearVelocity += impulse * bodyB.Body.InverseMass;
    }
}