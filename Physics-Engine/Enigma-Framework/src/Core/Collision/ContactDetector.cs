using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Collision;

public static class ContactDetector
{
    public static void FindContactPoints(Collider bodyA, Collider bodyB, out Vector2 contact1,
        out Vector2 contact2, out int contactCount)
    {
        contact1 = Vector2.Zero;
        contact2 = Vector2.Zero;
        contactCount = 0;
        if (bodyA is CircleCollider circleA && bodyB is CircleCollider circleB)
        {
            FindCircleToCircleContactPoint(circleA, circleB, out contact1);
            contactCount = 1;
        }

        if (bodyA is CircleCollider circleA1 && bodyB is PolygonCollider boxB)
        {
            FindPolygonToCircleContactPoint(boxB, circleA1, out contact1);
            contactCount = 1;
        }

        if (bodyA is PolygonCollider boxA1 && bodyB is CircleCollider circleB2)
        {
            FindPolygonToCircleContactPoint(boxA1, circleB2,
                out contact1);
            contactCount = 1;
        }

        if (bodyA is PolygonCollider boxA2 && bodyB is PolygonCollider boxB1)
            FindPolygonToPolygonPoint(boxA2, boxB1, out contact1, out contact2,
                out contactCount);
    }

    private static void FindCircleToCircleContactPoint(CircleCollider circleA, CircleCollider circleB,
        out Vector2 contactPoint)
    {
        var ab = circleB.Position - circleA.Position;
        var direction = Mathematics.Normalize(ab);
        var radius = circleA.CircleArea.Radius;
        contactPoint = circleA.Position + direction * radius;
    }

    private static void FindPolygonToCircleContactPoint(PolygonCollider polygon, CircleCollider circle,
        out Vector2 contactPoint)
    {
        contactPoint = Vector2.Zero;
        var vertices = polygon.GetTransformedVertices();

        float minDistSq = float.MaxValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 a = vertices[i];
            Vector2 b = vertices[(i + 1) % vertices.Length];

            Vector2 ab = b - a;
            float t = Mathematics.Clamp(Mathematics.DotProduct(circle.Position - a, ab) / Mathematics.DotProduct(ab, ab), 0f, 1f);
            Vector2 projection = a + ab * t;

            float distSq = Mathematics.DistanceSq(circle.Position, projection);
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                contactPoint = projection;
            }
        }
    }

    private static void FindPolygonToPolygonPoint(PolygonCollider polyA, PolygonCollider polyB,
        out Vector2 contactPoint1, out Vector2 contactPoint2, out int contactCount)
    {
        // defaults
        contactPoint1 = Vector2.Zero;
        contactPoint2 = Vector2.Zero;
        contactCount = 0;

        var vertsA = polyA.GetTransformedVertices();
        var vertsB = polyB.GetTransformedVertices();

        // 1. Determine reference and incident faces from SAT normal
        // Recompute SAT normal here (simple version):
        Vector2 normal = Vector2.Zero;
        float depth = float.MaxValue;

        var aVerts = polyA.GetTransformedVertices();
        var bVerts = polyB.GetTransformedVertices();

        ComputeSATNormal(aVerts, bVerts, ref normal, ref depth);

        bool flip;

        int referenceIndex = FindReferenceFace(vertsA, normal, out flip);
        int incidentIndex = FindIncidentFace(vertsB, normal);

        // Get the actual edges
        Vector2 refA = vertsA[referenceIndex];
        Vector2 refB = vertsA[(referenceIndex + 1) % vertsA.Length];

        Vector2 incA = vertsB[incidentIndex];
        Vector2 incB = vertsB[(incidentIndex + 1) % vertsB.Length];

        // 2. Reference side planes
        Vector2 refDir = Mathematics.Normalize(refB - refA);
        Vector2 refNormal = new Vector2(-refDir.y, refDir.x);
        if (flip) refNormal = -refNormal;

        float refOffset = Mathematics.DotProduct(refNormal, refA);

        // 3. Clip incident segment against reference planes (FlatPhysics clipping)
        Vector2[] clipped = new Vector2[2] { incA, incB };
        int count = Clip(refDir, refOffset, clipped, out var clipped1, out var clipped2);

        if (count == 0)
            return;

        // 4. Output final contact points
        contactPoint1 = clipped1;
        if (count > 1)
        {
            contactPoint2 = clipped2;
            contactCount = 2;
        }
        else
        {
            contactCount = 1;
        }
    }
    private static void ComputeSATNormal(Vector2[] vertsA, Vector2[] vertsB,
        ref Vector2 bestNormal, ref float bestDepth)
    {
        bestDepth = float.MaxValue;
        bestNormal = Vector2.Zero;

        // Check axes from polygon A
        for (int i = 0; i < vertsA.Length; i++)
        {
            Vector2 a = vertsA[i];
            Vector2 b = vertsA[(i + 1) % vertsA.Length];
            Vector2 edge = b - a;
            Vector2 axis = new Vector2(-edge.y, edge.x).Normalized();

            if (!CheckAxis(axis, vertsA, vertsB, ref bestNormal, ref bestDepth)) 
                return;
        }

        // Check axes from polygon B
        for (int i = 0; i < vertsB.Length; i++)
        {
            Vector2 a = vertsB[i];
            Vector2 b = vertsB[(i + 1) % vertsB.Length];
            Vector2 edge = b - a;
            Vector2 axis = new Vector2(-edge.y, edge.x).Normalized();

            if (!CheckAxis(axis, vertsA, vertsB, ref bestNormal, ref bestDepth)) 
                return;
        }
    }

    private static bool CheckAxis(Vector2 axis, Vector2[] vertsA, Vector2[] vertsB,
        ref Vector2 bestNormal, ref float bestDepth)
    {
        Project(vertsA, axis, out float minA, out float maxA);
        Project(vertsB, axis, out float minB, out float maxB);

        if (minA >= maxB || minB >= maxA)
            return false;

        float overlap = System.Math.Min(maxA - minB, maxB - minA);
        if (overlap < bestDepth)
        {
            bestDepth = overlap;
            bestNormal = axis;
        }

        return true;
    }

    private static void Project(Vector2[] verts, Vector2 axis, out float min, out float max)
    {
        min = max = Vector2.Dot(verts[0], axis);
        for (int i = 1; i < verts.Length; i++)
        {
            float p = Vector2.Dot(verts[i], axis);
            if (p < min) min = p;
            if (p > max) max = p;
        }
    }

    private static int FindReferenceFace(Vector2[] verts, Vector2 normal, out bool flip)
    {
        int index = 0;
        float minDot = float.MaxValue;

        for (int i = 0; i < verts.Length; i++)
        {
            Vector2 a = verts[i];
            Vector2 b = verts[(i + 1) % verts.Length];
            Vector2 edge = Mathematics.Normalize(b - a);

            float dot = System.Math.Abs(Vector2.Dot(edge, normal));
            if (dot < minDot)
            {
                minDot = dot;
                index = i;
            }
        }

        flip = Vector2.Dot(ComputeEdgeNormal(verts, index), normal) < 0;
        return index;
    }
    private static Vector2 ComputeEdgeNormal(Vector2[] verts, int index)
    {
        Vector2 a = verts[index];
        Vector2 b = verts[(index + 1) % verts.Length];
        Vector2 edge = b - a;

        return new Vector2(-edge.y, edge.x).Normalized();
    }

    private static int FindIncidentFace(Vector2[] verts, Vector2 normal)
    {
        int index = 0;
        float minDot = float.MaxValue;

        for (int i = 0; i < verts.Length; i++)
        {
            Vector2 a = verts[i];
            Vector2 b = verts[(i + 1) % verts.Length];
            Vector2 edge = Mathematics.Normalize(b - a);
            Vector2 edgeNormal = new Vector2(-edge.y, edge.x);

            float dot = Vector2.Dot(edgeNormal, normal);
            if (dot < minDot)
            {
                minDot = dot;
                index = i;
            }
        }

        return index;
    }

    private static int Clip(Vector2 normal, float offset, Vector2[] line,
        out Vector2 out1, out Vector2 out2)
    {
        out1 = Vector2.Zero;
        out2 = Vector2.Zero;

        Vector2 pA = line[0];
        Vector2 pB = line[1];

        float dA = Vector2.Dot(normal, pA) - offset;
        float dB = Vector2.Dot(normal, pB) - offset;

        int count = 0;

        if (dA >= 0)
            out1 = pA;

        if (dB >= 0)
            if (count == 0)
                out1 = pB;
            else
                out2 = pB;

        if (dA * dB < 0)
        {
            float t = dA / (dA - dB);
            Vector2 intersection = pA + (pB - pA) * t;

            if (count == 0)
                out1 = intersection;
            else
                out2 = intersection;

            count++;
        }

        return count;
    }

    private static void PointSegmentDistance(Vector2 point, Vector2 edgeStartPoint, Vector2 edgeEndPoint,
        out float distanceSquared, out Vector2 closetsPoint)
    {
        closetsPoint = Vector2.Zero;

        var edgeDirection = edgeEndPoint - edgeStartPoint;
        var startToPoint = point - edgeStartPoint;

        var project = Mathematics.DotProduct(startToPoint, edgeDirection);
        var edgeDirectionLengthSquared = Mathematics.LengthSq(edgeDirection);
        var d = project / edgeDirectionLengthSquared;
        if (d <= 0)
            closetsPoint = edgeStartPoint;
        else if (d >= 1)
            closetsPoint = edgeEndPoint;
        else
            closetsPoint = edgeStartPoint + edgeDirection * d;

        distanceSquared = Mathematics.DistanceSq(point, closetsPoint);
    }
}