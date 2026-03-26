using System.Text;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision;

public static class CollisionResolver
{
    public static void ResolveBasic(in CollisionManifold contact)
    {
        var bodyA = contact.BodyA;
        var bodyB = contact.BodyB;
        var colA = contact.ColliderA;
        var colB = contact.ColliderB;
        var collisionInfo = contact.CollisionInfo;

        Vector2 relativeVelocity = bodyB.Body.LinearVelocity - bodyA.Body.LinearVelocity;

        bool isNormalAndVelocityInSameDirection = Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal) > 0f;
        if (isNormalAndVelocityInSameDirection)
        {
            return;
        }

        float e = System.Math.Min(colA.Material.Restitution, colB.Material.Restitution);
        float j = -(1f + e) * Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal);
        j /= bodyA.Body.InverseMass + bodyB.Body.InverseMass;

        Vector2 impulse = j * collisionInfo.Normal;
        bodyA.Body.LinearVelocity -= impulse * bodyA.Body.InverseMass;
        bodyB.Body.LinearVelocity += impulse * bodyB.Body.InverseMass;
    }

    public static void ResolveWithRotation(in CollisionManifold contact)
    {
        var bodyA = contact.BodyA;
        var bodyB = contact.BodyB;
        var colA = contact.ColliderA;
        var colB = contact.ColliderB;
        var collisionInfo = contact.CollisionInfo;
        var contactCount = contact.ContactCount;
        float e = System.Math.Min(colA.Material.Restitution, colB.Material.Restitution);

        Vector2[] contacts = { contact.Contact1, contact.Contact2 };

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 ra = contacts[i] - bodyA.Position;
            Vector2 rb = contacts[i] - bodyB.Position;

            Vector2 raPerp = new Vector2(-ra.y, ra.x);
            Vector2 rbPerp = new Vector2(-rb.y, rb.x);

            Vector2 angularLinearVelocityA = bodyA.Body.AngularVelocity * raPerp;
            Vector2 angularLinearVelocityB = bodyB.Body.AngularVelocity * rbPerp;

            Vector2 relativeVelocity = (bodyB.Body.LinearVelocity + angularLinearVelocityB) -
                                       (bodyA.Body.LinearVelocity + angularLinearVelocityA);

            float contactVelocityMagnitude = Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal);
            if (contactVelocityMagnitude > 0f)
            {
                continue;
            }

            float raPerpDotNormal = Mathematics.DotProduct(raPerp, collisionInfo.Normal);
            float rbPerpDotNormal = Mathematics.DotProduct(rbPerp, collisionInfo.Normal);

            float denominator = bodyA.Body.InverseMass + bodyB.Body.InverseMass +
                                (raPerpDotNormal * raPerpDotNormal * bodyA.Body.InverseInertia) +
                                (rbPerpDotNormal * rbPerpDotNormal * bodyB.Body.InverseInertia);

            if (System.Math.Abs(denominator) < 1e-10f)
            {
                continue;
            }

            float j = -(1f + e) * contactVelocityMagnitude;
            j /= denominator;

            Vector2 impulse = j * collisionInfo.Normal;

            bodyA.Body.LinearVelocity -= impulse * bodyA.Body.InverseMass;
            bodyB.Body.LinearVelocity += impulse * bodyB.Body.InverseMass;

            float crossA = Mathematics.CrossProduct(ra, impulse);
            float crossB = Mathematics.CrossProduct(rb, impulse);

            bodyA.Body.AngularVelocity -= crossA * bodyA.Body.InverseInertia;
            bodyB.Body.AngularVelocity += crossB * bodyB.Body.InverseInertia;
        }
    }

    public static void ResolveWithRotationWithFriction(in CollisionManifold contact)
    {
        var bodyA = contact.BodyA;
        var bodyB = contact.BodyB;
        var colA = contact.ColliderA;
        var colB = contact.ColliderB;
        var collisionInfo = contact.CollisionInfo;
        var contactCount = contact.ContactCount;
        float e = System.Math.Min(colA.Material.Restitution, colB.Material.Restitution);

        Vector2[] contacts = { contact.Contact1, contact.Contact2 };
        float[] jList = new float[2];

        float staticFriction = (colA.Material.StaticFriction + colB.Material.StaticFriction) / 2f;
        float dynamicFriction = (colA.Material.DynamicFriction + colB.Material.DynamicFriction) / 2f;

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 ra = contacts[i] - bodyA.Position;
            Vector2 rb = contacts[i] - bodyB.Position;

            Vector2 raPerp = new Vector2(-ra.y, ra.x);
            Vector2 rbPerp = new Vector2(-rb.y, rb.x);

            Vector2 angularLinearVelocityA = bodyA.Body.AngularVelocity * raPerp;
            Vector2 angularLinearVelocityB = bodyB.Body.AngularVelocity * rbPerp;

            Vector2 relativeVelocity = (bodyB.Body.LinearVelocity + angularLinearVelocityB) -
                                       (bodyA.Body.LinearVelocity + angularLinearVelocityA);

            float contactVelocityMagnitude = Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal);
            if (contactVelocityMagnitude > 0f)
            {
                jList[i] = 0f;
                continue;
            }

            float raPerpDotNormal = Mathematics.DotProduct(raPerp, collisionInfo.Normal);
            float rbPerpDotNormal = Mathematics.DotProduct(rbPerp, collisionInfo.Normal);

            float denominator = bodyA.Body.InverseMass + bodyB.Body.InverseMass +
                                (raPerpDotNormal * raPerpDotNormal * bodyA.Body.InverseInertia) +
                                (rbPerpDotNormal * rbPerpDotNormal * bodyB.Body.InverseInertia);

            if (System.Math.Abs(denominator) < 1e-10f)
            {
                jList[i] = 0f;
                continue;
            }

            float j = -(1f + e) * contactVelocityMagnitude;
            j /= denominator;
            jList[i] = j;
        }

        if (contactCount > 0)
        {
            float avgJ = 0f;
            for (int i = 0; i < contactCount; i++)
            {
                avgJ += jList[i];
            }

            avgJ /= contactCount;

            Vector2 impulse = avgJ * collisionInfo.Normal;

            bodyA.Body.LinearVelocity -= impulse * bodyA.Body.InverseMass;
            bodyB.Body.LinearVelocity += impulse * bodyB.Body.InverseMass;

            for (int i = 0; i < contactCount; i++)
            {
                Vector2 ra = contacts[i] - bodyA.Position;
                Vector2 rb = contacts[i] - bodyB.Position;

                float crossA = Mathematics.CrossProduct(ra, impulse) / contactCount;
                float crossB = Mathematics.CrossProduct(rb, impulse) / contactCount;

                bodyA.Body.AngularVelocity -= crossA * bodyA.Body.InverseInertia;
                bodyB.Body.AngularVelocity += crossB * bodyB.Body.InverseInertia;
            }
        }

        for (int i = 0; i < contactCount; i++)
        {
            if (jList[i] == 0f) continue;

            Vector2 ra = contacts[i] - bodyA.Position;
            Vector2 rb = contacts[i] - bodyB.Position;

            Vector2 raPerp = new Vector2(-ra.y, ra.x);
            Vector2 rbPerp = new Vector2(-rb.y, rb.x);

            Vector2 angularLinearVelocityA = bodyA.Body.AngularVelocity * raPerp;
            Vector2 angularLinearVelocityB = bodyB.Body.AngularVelocity * rbPerp;

            Vector2 relativeVelocity = (bodyB.Body.LinearVelocity + angularLinearVelocityB) -
                                       (bodyA.Body.LinearVelocity + angularLinearVelocityA);

            Vector2 tangent = relativeVelocity -
                              Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal) * collisionInfo.Normal;

            if (Mathematics.IsNearlyEqual(tangent, Vector2.Zero))
            {
                continue;
            }

            tangent = Mathematics.Normalize(tangent);

            float raPerpDotTangent = Mathematics.DotProduct(raPerp, tangent);
            float rbPerpDotTangent = Mathematics.DotProduct(rbPerp, tangent);

            float denominator = bodyA.Body.InverseMass + bodyB.Body.InverseMass +
                                (raPerpDotTangent * raPerpDotTangent * bodyA.Body.InverseInertia) +
                                (rbPerpDotTangent * rbPerpDotTangent * bodyB.Body.InverseInertia);

            if (System.Math.Abs(denominator) < 1e-10f)
            {
                continue;
            }

            float jt = -Mathematics.DotProduct(relativeVelocity, tangent);
            jt /= denominator;

            Vector2 impulseFriction;

            if (System.Math.Abs(jt) <= jList[i] * staticFriction)
            {
                impulseFriction = jt * tangent;
            }
            else
            {
                impulseFriction = -jList[i] * tangent * dynamicFriction;
            }

            bodyA.Body.LinearVelocity -= impulseFriction * bodyA.Body.InverseMass;
            bodyB.Body.LinearVelocity += impulseFriction * bodyB.Body.InverseMass;

            float crossA = Mathematics.CrossProduct(ra, impulseFriction);
            float crossB = Mathematics.CrossProduct(rb, impulseFriction);

            bodyA.Body.AngularVelocity -= crossA * bodyA.Body.InverseInertia;
            bodyB.Body.AngularVelocity += crossB * bodyB.Body.InverseInertia;
        }
    }
}