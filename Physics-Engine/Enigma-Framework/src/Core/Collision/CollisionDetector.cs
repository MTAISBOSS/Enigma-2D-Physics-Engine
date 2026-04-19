using Enigma_Framework.Math;
using Enigma_Framework.Utilities;

namespace Enigma_Framework.Core.Collision;

public static class CollisionDetector
{
    public static bool IntersectAABBs(AABBCollision bodyA, AABBCollision bodyB)
    {
        if (bodyA.Max.x <= bodyB.Min.x || bodyB.Max.x <= bodyA.Min.x) return false;
        if (bodyA.Max.y <= bodyB.Min.y || bodyB.Max.y <= bodyA.Min.y) return false;
        return true;
    }

    public static bool Intersect(Collider colA, Collider colB, out CollisionInfo collisionInfo)
    {
        collisionInfo = new CollisionInfo();
        if ((colA.Position - colB.Position).Equals(Vector2.Zero))
        {
            collisionInfo.Depth = 1;
            collisionInfo.Normal =
                new Vector2(RandomHelper.GetRandomFloat(0, 1), RandomHelper.GetRandomFloat(0, 1));
            return true;
        }

        if (colA is CircleCollider circleA && colB is CircleCollider circleB)
            return IntersectCircles(circleA, circleB, out collisionInfo);

        if (colA is CircleCollider circleA1 && colB is PolygonCollider boxB)
            return IntersectCircleWithPolygon(circleA1, boxB,
                out collisionInfo);

        if (colA is PolygonCollider boxA && colB is CircleCollider circleB1)
        {
            var result = IntersectCircleWithPolygon(circleB1, boxA,
                out collisionInfo);
            collisionInfo.Normal = -collisionInfo.Normal;
            return result;
        }

        if (colA is PolygonCollider boxA1 && colB is PolygonCollider boxB1)
            return IntersectPolygons(boxA1, boxB1, out collisionInfo);

        collisionInfo = new CollisionInfo();
        return false;
    }

    #region Intersection Algorithms

    private static bool IntersectCircles(CircleCollider circleA, CircleCollider circleB, out CollisionInfo collisionInfo)
    {
        collisionInfo = new CollisionInfo();
        Vector2 ab = circleB.Position - circleA.Position;
        float distance = Mathematics.Length(ab);
        float totalRadius = circleA.CircleArea.Radius + circleB.CircleArea.Radius;

        if (distance >= totalRadius)
            return false;

        collisionInfo.Depth = totalRadius - distance;
        collisionInfo.Normal = distance > 0
            ? ab / distance
            : new Vector2(1, 0);

        return true;
    }


    private static bool IntersectCircleWithPolygon(CircleCollider circle, PolygonCollider polygon, out CollisionInfo collisionInfo)
    {
        collisionInfo = new CollisionInfo
        {
            Normal = Vector2.Zero,
            Depth = 0
        };

        var vertices = polygon.GetTransformedVertices();
        Vector2 closestPoint = vertices[0];
        float minDistSq = float.MaxValue;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 a = vertices[i];
            Vector2 b = vertices[(i + 1) % vertices.Length];
            Vector2 ab = b - a;
            float t = Mathematics.Clamp(Mathematics.DotProduct(circle.Position - a, ab) / Mathematics.DotProduct(ab, ab), 0, 1);
            Vector2 projection = a + ab * t;
            float distSq = Mathematics.LengthSq(projection - circle.Position);
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closestPoint = projection;
            }
        }

        float dist = MathF.Sqrt(minDistSq);
        if (dist >= circle.CircleArea.Radius)
            return false;

        collisionInfo.Normal = Mathematics.Normalize(closestPoint - circle.Position);
        collisionInfo.Depth = circle.CircleArea.Radius - dist;
        return true;
    }

    private static bool IntersectPolygons(PolygonCollider polygonA, PolygonCollider polygonB, out CollisionInfo collisionInfo)
    {
        collisionInfo = new CollisionInfo
        {
            Depth = float.MaxValue,
            Normal = Vector2.Zero
        };

        var verticesA = polygonA.GetTransformedVertices();
        var verticesB = polygonB.GetTransformedVertices();

        for (int shapeIndex = 0; shapeIndex < 2; shapeIndex++)
        {
            var currentVerts = shapeIndex == 0 ? verticesA : verticesB;
            for (int i = 0; i < currentVerts.Length; i++)
            {
                Vector2 a = currentVerts[i];
                Vector2 b = currentVerts[(i + 1) % currentVerts.Length];
                Vector2 edge = b - a;
                Vector2 axis = new Vector2(-edge.y, edge.x);
                axis = Mathematics.Normalize(axis);

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                    return false;

                float overlapAtoB = maxA - minB;
                float overlapBtoA = maxB - minA;
                float axisDepth = overlapAtoB < overlapBtoA ? overlapAtoB : overlapBtoA;

                if (axisDepth < collisionInfo.Depth)
                {
                    collisionInfo.Depth = axisDepth;
                    collisionInfo.Normal = axis;
                }
            }
        }

        Vector2 direction = polygonB.Position - polygonA.Position;
        if (Mathematics.DotProduct(direction, collisionInfo.Normal) < 0f)
            collisionInfo.Normal = -collisionInfo.Normal;

        return true;
    }

    #endregion

    #region Helper Methods

    private static int FindClosestPoint(Vector2[] vertices, Vector2 circleCenter)
    {
        var index = -1;
        var minDistance = float.MaxValue;
        for (var i = 0; i < vertices.Length; i++)
        {
            var dist = Mathematics.Distance(vertices[i], circleCenter);
            if (dist < minDistance)
            {
                minDistance = dist;
                index = i;
            }
        }

        return index;
    }

    private static void ProjectCircle(Vector2 center, float radius, Vector2 axis, out float min, out float max)
    {
        var direction = Mathematics.Normalize(axis);
        var directionAndRadius = direction * radius;

        var p1 = center + directionAndRadius;
        var p2 = center - directionAndRadius;

        min = Mathematics.DotProduct(p2, axis);
        max = Mathematics.DotProduct(p1, axis);

        if (max < min) (min, max) = (max, min);
    }

    private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
    {
        max = float.MinValue;
        min = float.MaxValue;

        for (var i = 0; i < vertices.Length; i++)
        {
            var v = vertices[i];
            var proj = Mathematics.DotProduct(v, axis);
            if (proj < min) min = proj;

            if (proj > max) max = proj;
        }
    }

    #endregion
}