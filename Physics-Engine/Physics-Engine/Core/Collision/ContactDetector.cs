using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision;

public struct ContactDetector
{
    
    public static void FindContactPoints(Rigidbody2D bodyA, Rigidbody2D bodyB, out Vector2 contact1,
        out Vector2 contact2, out int contactCount)
    {
        
        contact1 = Vector2.Zero;
        contact2 = Vector2.Zero;
        contactCount = 0; 
        if (bodyA is CircleRigidbody2D && bodyB is CircleRigidbody2D)
        {
            contactCount = 1;
            FindCircleToCircleContactPoint(bodyA as CircleRigidbody2D, (CircleRigidbody2D)bodyB, out contact1);
        }

        if (bodyA is CircleRigidbody2D && bodyB is BoxRigidbody2D)
        {
            // IntersectCircleWithPolygon(bodyA as CircleRigidbody2D, bodyB as BoxRigidbody2D,
            //     out collisionInfo);
        }

        if (bodyA is BoxRigidbody2D && bodyB is CircleRigidbody2D)
        {
            // IntersectCircleWithPolygon(bodyB as CircleRigidbody2D, bodyA as BoxRigidbody2D,
            //     out collisionInfo);
            // collisionInfo.Normal = -collisionInfo.Normal;
        }

        if (bodyA is BoxRigidbody2D && bodyB is BoxRigidbody2D)
        {
            // IntersectPolygons(bodyA as BoxRigidbody2D, bodyB as BoxRigidbody2D, out collisionInfo);
        }

        // collisionInfo = new CollisionInfo();
    }
    private static void FindCircleToCircleContactPoint(Rigidbody2D circleA, Rigidbody2D circleB, out Vector2 contactPoint)
    {
        Vector2 ab = circleB.Position - circleA.Position;
        Vector2 direction = Mathematics.Normalize(ab);
        float radius = circleA.Owner.Components.Get<CircleRigidbody2D>().CircleArea.Radius;
        contactPoint = circleA.Position + direction * radius;
    }
}