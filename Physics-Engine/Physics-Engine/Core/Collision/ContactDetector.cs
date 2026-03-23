using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision;

public static class ContactDetector
{
    public static void FindContactPoints(Rigidbody2D bodyA, Rigidbody2D bodyB, out Vector2 contact1,
        out Vector2 contact2, out int contactCount)
    {
        contact1 = Vector2.Zero;
        contact2 = Vector2.Zero;
        contactCount = 0;
        if (bodyA is CircleRigidbody2D circleA && bodyB is CircleRigidbody2D circleB)
        {
            FindCircleToCircleContactPoint(circleA, circleB, out contact1);
            contactCount = 1;
        }

        if (bodyA is CircleRigidbody2D circleA1 && bodyB is BoxRigidbody2D boxB)
        {
            FindPolygonToCircleContactPoint(boxB, circleA1, out contact1);
            contactCount = 1;
        }

        if (bodyA is BoxRigidbody2D boxA1 && bodyB is CircleRigidbody2D circleB2)
        {
            FindPolygonToCircleContactPoint(boxA1, circleB2,
                out contact1);
            contactCount = 1;
        }

        if (bodyA is BoxRigidbody2D boxA2 && bodyB is BoxRigidbody2D boxB1)
        {
            FindPolygonToPolygonPoint(boxA2, boxB1, out contact1, out contact2,
                out contactCount);
        }
    }

    private static void FindCircleToCircleContactPoint(CircleRigidbody2D circleA, CircleRigidbody2D circleB,
        out Vector2 contactPoint)
    {
        Vector2 ab = circleB.Position - circleA.Position;
        Vector2 direction = Mathematics.Normalize(ab);
        float radius = circleA.CircleArea.Radius;
        contactPoint = circleA.Position + direction * radius;
    }

    private static void FindPolygonToCircleContactPoint(BoxRigidbody2D polygon, CircleRigidbody2D circle,
        out Vector2 contactPoint)
    {
        float minDistance = float.MaxValue;
        contactPoint = Vector2.Zero;
        var vertices = polygon.GetTransformedVertices();
        if (vertices != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 va = vertices[i];
                Vector2 vb = vertices[i + 1 == vertices.Length ? 0 : i + 1];
                PointSegmentDistance(circle.Position, va, vb, out float distanceSquared, out Vector2 closetsPoint);
                if (distanceSquared < minDistance)
                {
                    minDistance = distanceSquared;
                    contactPoint = closetsPoint;
                }
            }
        }
    }

    static void FindPolygonToPolygonPoint(BoxRigidbody2D polygon1, BoxRigidbody2D polygon2,
        out Vector2 contactPoint1, out Vector2 contactPoint2, out int contactCount)
    {
        contactPoint1 = Vector2.Zero;
        contactPoint2 = Vector2.Zero;
        contactCount = 0;
        var verticesA = polygon1.GetTransformedVertices();
        var verticesB = polygon2.GetTransformedVertices();
        float minDistance = float.MaxValue;
        for (int i = 0; i < verticesA.Length; i++)
        {
            Vector2 point = verticesA[i];
            for (int j = 0; j < verticesB.Length; j++)
            {
                Vector2 va = verticesB[j];
                Vector2 vb = verticesB[i + 1 == verticesB.Length ? 0 : i + 1];
                PointSegmentDistance(point, va, vb, out float distanceSquared, out Vector2 closetsPoint);
                if (Mathematics.IsNearlyEqual(distanceSquared, minDistance))
                {
                    if (!Mathematics.IsNearlyEqual(contactPoint1, closetsPoint))
                    {
                        contactPoint2 = closetsPoint;
                        contactCount = 2;
                    }
                }
                else if (distanceSquared < minDistance)
                {
                    minDistance = distanceSquared;
                    contactCount = 1;
                    contactPoint1 = closetsPoint;
                }
            }
        }

        for (int i = 0; i < verticesB.Length; i++)
        {
            Vector2 point = verticesB[i];
            for (int j = 0; j < verticesA.Length; j++)
            {
                Vector2 va = verticesA[j];
                Vector2 vb = verticesA[i + 1 == verticesA.Length ? 0 : i + 1];
                PointSegmentDistance(point, va, vb, out float distanceSquared, out Vector2 closetsPoint);
                if (Mathematics.IsNearlyEqual(distanceSquared, minDistance))
                {
                    if (!Mathematics.IsNearlyEqual(contactPoint1, closetsPoint))
                    {
                        contactPoint2 = closetsPoint;
                        contactCount = 2;
                    }
                }
                else if (distanceSquared < minDistance)
                {
                    minDistance = distanceSquared;
                    contactCount = 1;
                    contactPoint1 = closetsPoint;
                }
            }
        }
    }

    static void PointSegmentDistance(Vector2 point, Vector2 edgeStartPoint, Vector2 edgeEndPoint,
        out float distanceSquared, out Vector2 closetsPoint)
    {
        closetsPoint = Vector2.Zero;

        Vector2 edgeDirection = edgeEndPoint - edgeStartPoint;
        Vector2 startToPoint = point - edgeStartPoint;

        float project = Mathematics.DotProduct(startToPoint, edgeDirection);
        float edgeDirectionLengthSquared = Mathematics.LengthSq(edgeDirection);
        float d = project / edgeDirectionLengthSquared;
        if (d <= 0)
        {
            closetsPoint = edgeStartPoint;
        }
        else if (d >= 1)
        {
            closetsPoint = edgeEndPoint;
        }
        else
        {
            closetsPoint = edgeStartPoint + edgeDirection * d;
        }

        distanceSquared = Mathematics.DistanceSq(point, closetsPoint);
    }
}