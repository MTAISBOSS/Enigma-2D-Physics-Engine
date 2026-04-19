using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Collision;

public static class CollisionResolver
{
    public static void ResolveBasic(in CollisionManifold contact)
    {
        var bodyA = contact.BodyA;
        var bodyB = contact.BodyB;
        var colA = contact.ColliderA;
        var colB = contact.ColliderB;
        var collisionInfo = contact.CollisionInfo;

        var relativeVelocity = bodyB.Body.LinearVelocity - bodyA.Body.LinearVelocity;

        var isNormalAndVelocityInSameDirection = Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal) > 0f;
        if (isNormalAndVelocityInSameDirection) return;

        var e = System.Math.Min(colA.Material.Restitution, colB.Material.Restitution);
        var j = -(1f + e) * Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal);
        j /= bodyA.Body.InverseMass + bodyB.Body.InverseMass;

        var impulse = j * collisionInfo.Normal;
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
        var e = System.Math.Min(colA.Material.Restitution, colB.Material.Restitution);

        Vector2[] contacts = { contact.Contact1, contact.Contact2 };

        for (var i = 0; i < contactCount; i++)
        {
            var ra = contacts[i] - bodyA.Position;
            var rb = contacts[i] - bodyB.Position;

            var raPerp = new Vector2(-ra.y, ra.x);
            var rbPerp = new Vector2(-rb.y, rb.x);

            var angularLinearVelocityA = bodyA.Body.AngularVelocity * raPerp;
            var angularLinearVelocityB = bodyB.Body.AngularVelocity * rbPerp;

            var relativeVelocity = bodyB.Body.LinearVelocity + angularLinearVelocityB -
                                   (bodyA.Body.LinearVelocity + angularLinearVelocityA);

            var contactVelocityMagnitude = Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal);
            if (contactVelocityMagnitude > 0f) continue;

            var raPerpDotNormal = Mathematics.DotProduct(raPerp, collisionInfo.Normal);
            var rbPerpDotNormal = Mathematics.DotProduct(rbPerp, collisionInfo.Normal);

            var denominator = bodyA.Body.InverseMass + bodyB.Body.InverseMass +
                              raPerpDotNormal * raPerpDotNormal * bodyA.Body.InverseInertia +
                              rbPerpDotNormal * rbPerpDotNormal * bodyB.Body.InverseInertia;

            if (System.Math.Abs(denominator) < 1e-10f) continue;

            var j = -(1f + e) * contactVelocityMagnitude;
            j /= denominator;
            j /= contactCount;


            var impulse = j * collisionInfo.Normal;

            bodyA.Body.LinearVelocity -= impulse * bodyA.Body.InverseMass;
            bodyB.Body.LinearVelocity += impulse * bodyB.Body.InverseMass;

            var crossA = Mathematics.CrossProduct(ra, impulse);
            var crossB = Mathematics.CrossProduct(rb, impulse);

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
        var e = System.Math.Min(colA.Material.Restitution, colB.Material.Restitution);

        Vector2[] contacts = { contact.Contact1, contact.Contact2 };
        var jList = new float[2];

        var staticFriction = System.MathF.Sqrt(
            colA.Material.StaticFriction * colB.Material.StaticFriction);

        var dynamicFriction = System.MathF.Sqrt(
            colA.Material.DynamicFriction * colB.Material.DynamicFriction);

        for (var i = 0; i < contactCount; i++)
        {
            var ra = contacts[i] - bodyA.Position;
            var rb = contacts[i] - bodyB.Position;

            var raPerp = new Vector2(-ra.y, ra.x);
            var rbPerp = new Vector2(-rb.y, rb.x);

            var angularLinearVelocityA = bodyA.Body.AngularVelocity * raPerp;
            var angularLinearVelocityB = bodyB.Body.AngularVelocity * rbPerp;

            var relativeVelocity = bodyB.Body.LinearVelocity + angularLinearVelocityB -
                                   (bodyA.Body.LinearVelocity + angularLinearVelocityA);

            var contactVelocityMagnitude = Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal);
            if (contactVelocityMagnitude > 0f)
            {
                jList[i] = 0f;
                continue;
            }

            var raPerpDotNormal = Mathematics.DotProduct(raPerp, collisionInfo.Normal);
            var rbPerpDotNormal = Mathematics.DotProduct(rbPerp, collisionInfo.Normal);

            var denominator = bodyA.Body.InverseMass + bodyB.Body.InverseMass +
                              raPerpDotNormal * raPerpDotNormal * bodyA.Body.InverseInertia +
                              rbPerpDotNormal * rbPerpDotNormal * bodyB.Body.InverseInertia;

            if (System.Math.Abs(denominator) < 1e-10f)
            {
                jList[i] = 0f;
                continue;
            }

            var j = -(1f + e) * contactVelocityMagnitude;
            j /= denominator;
            j /= contactCount;

            jList[i] = j;
        }

        if (contactCount > 0)
        {
            var avgJ = 0f;
            for (var i = 0; i < contactCount; i++) avgJ += jList[i];

            avgJ /= contactCount;

            var impulse = avgJ * collisionInfo.Normal;

            bodyA.Body.LinearVelocity -= impulse * bodyA.Body.InverseMass;
            bodyB.Body.LinearVelocity += impulse * bodyB.Body.InverseMass;

            for (var i = 0; i < contactCount; i++)
            {
                var ra = contacts[i] - bodyA.Position;
                var rb = contacts[i] - bodyB.Position;

                var crossA = Mathematics.CrossProduct(ra, impulse) / contactCount;
                var crossB = Mathematics.CrossProduct(rb, impulse) / contactCount;

                bodyA.Body.AngularVelocity -= crossA * bodyA.Body.InverseInertia;
                bodyB.Body.AngularVelocity += crossB * bodyB.Body.InverseInertia;
            }
        }

        for (var i = 0; i < contactCount; i++)
        {
            if (jList[i] == 0f) continue;

            var ra = contacts[i] - bodyA.Position;
            var rb = contacts[i] - bodyB.Position;

            var raPerp = new Vector2(-ra.y, ra.x);
            var rbPerp = new Vector2(-rb.y, rb.x);

            var angularLinearVelocityA = bodyA.Body.AngularVelocity * raPerp;
            var angularLinearVelocityB = bodyB.Body.AngularVelocity * rbPerp;

            var relativeVelocity = bodyB.Body.LinearVelocity + angularLinearVelocityB -
                                   (bodyA.Body.LinearVelocity + angularLinearVelocityA);

            var tangent = relativeVelocity -
                          Mathematics.DotProduct(relativeVelocity, collisionInfo.Normal) * collisionInfo.Normal;

            float lenSq = tangent.LengthSquared();

            if (lenSq < 1e-8f)
                continue;

            tangent /= System.MathF.Sqrt(lenSq);


            var raPerpDotTangent = Mathematics.DotProduct(raPerp, tangent);
            var rbPerpDotTangent = Mathematics.DotProduct(rbPerp, tangent);

            var denominator = bodyA.Body.InverseMass + bodyB.Body.InverseMass +
                              raPerpDotTangent * raPerpDotTangent * bodyA.Body.InverseInertia +
                              rbPerpDotTangent * rbPerpDotTangent * bodyB.Body.InverseInertia;

            if (System.Math.Abs(denominator) < 1e-10f) continue;

            var jt = -Mathematics.DotProduct(relativeVelocity, tangent);
            jt /= denominator;

            Vector2 impulseFriction;

            if (System.Math.Abs(jt) <= jList[i] * staticFriction)
                impulseFriction = jt * tangent;
            else
                impulseFriction = -jList[i] * tangent * dynamicFriction;

            bodyA.Body.LinearVelocity -= impulseFriction * bodyA.Body.InverseMass;
            bodyB.Body.LinearVelocity += impulseFriction * bodyB.Body.InverseMass;

            var crossA = Mathematics.CrossProduct(ra, impulseFriction);
            var crossB = Mathematics.CrossProduct(rb, impulseFriction);

            bodyA.Body.AngularVelocity -= crossA * bodyA.Body.InverseInertia;
            bodyB.Body.AngularVelocity += crossB * bodyB.Body.InverseInertia;
        }
    }
}