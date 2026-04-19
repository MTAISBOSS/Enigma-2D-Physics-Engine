using Enigma_Framework.Core.Collision;
using Enigma_Framework.Math;

namespace Enigma_Framework.Core.Raycast;

public static class Raycast2D
{
    public static bool Raycast(Ray2D ray2D, List<Collider> colliders, out RaycastHit2D hitInfo)
    {
        hitInfo = default;
        ray2D.Direction = Mathematics.Normalize(ray2D.Direction);
        var closestDistance = ray2D.Lenght;
        var hit = false;

        foreach (var collider in colliders)
            if (collider is CircleCollider circle)
            {
                if (RayIntersectCircle(ray2D.Origin, ray2D.Direction, circle, out var result) &&
                    result.Distance < closestDistance)
                {
                    closestDistance = result.Distance;
                    hitInfo = result;
                    hit = true;
                }
            }
            else if (collider is PolygonCollider polygon)
            {
                if (RayIntersectPolygon(ray2D.Origin, ray2D.Direction, polygon, out var result) &&
                    result.Distance < closestDistance)
                {
                    closestDistance = result.Distance;
                    hitInfo = result;
                    hit = true;
                }
            }

        return hit;
    }

    private static bool RayIntersectPolygon(Vector2 origin, Vector2 direction, PolygonCollider polygon,
        out RaycastHit2D hit)
    {
        hit = default;

        var found = false;
        var closestT = float.MaxValue;
        var bestPoint = Vector2.Zero;
        var bestNormal = Vector2.Zero;

        var vertices = polygon.TransformedVertices;

        for (var i = 0; i < vertices.Length; i++)
        {
            var a = vertices[i];
            var b = vertices[(i + 1) % vertices.Length];

            if (RayIntersectSegment(origin, direction, a, b, out var t, out var point))
                if (t >= 0 && t < closestT)
                {
                    closestT = t;
                    bestPoint = point;

                    var edge = b - a;
                    bestNormal = new Vector2(-edge.y, edge.x);
                    bestNormal = Mathematics.Normalize(bestNormal);

                    found = true;
                }
        }

        if (found)
        {
            hit.Collider = polygon;
            hit.Distance = closestT;
            hit.Point = bestPoint;
            hit.Normal = bestNormal;
            return true;
        }

        return false;
    }

    private static bool RayIntersectSegment(Vector2 origin, Vector2 direction, Vector2 a, Vector2 b,
        out float t, out Vector2 intersection)
    {
        intersection = Vector2.Zero;
        t = 0f;

        var e = b - a;
        var denom = Mathematics.CrossProduct(direction, e);

        if (System.Math.Abs(denom) < 1e-6f)
            return false;

        var aToOrigin = a - origin;

        t = Mathematics.CrossProduct(aToOrigin, e) / denom;
        var u = Mathematics.CrossProduct(aToOrigin, direction) / denom;

        if (t >= 0 && u >= 0 && u <= 1)
        {
            intersection = origin + direction * t;
            return true;
        }

        return false;
    }

    private static bool RayIntersectCircle(Vector2 origin, Vector2 direction, CircleCollider circle,
        out RaycastHit2D hit)
    {
        // (D⋅D)t^2 +2D⋅(O−C)t+(O−C)⋅(O−C)−r^2 =0
        hit = default;
        var oc = origin - circle.Position;
        var radius = circle.CircleArea.Radius;

        var a = Mathematics.LengthSq(direction);
        var b = 2f * Mathematics.DotProduct(oc, direction);
        var c = Mathematics.LengthSq(oc) - radius * radius;
        //delta = b^2 - 4ac
        //(-b+sqrt(delta)) / 2a
        //(-b-sqrt(delta)) / 2a

        var delta = b * b - 4f * a * c;
        if (delta < 0) return false;

        var deltaSqrt = (float)System.Math.Sqrt(delta);
        var t1 = (-b + deltaSqrt) / (2f * a);
        var t2 = (-b - deltaSqrt) / (2f * a);
        float t;
        if (t1 >= 0)
        {
            t = t1;
        }
        else
        {
            if (t2 >= 0)
                t = t2;
            else
                t = -1;
        }


        if (t < 0) return false;

        hit.Collider = circle;
        hit.Distance = t;
        hit.Point = origin + direction * t;
        hit.Normal = Mathematics.Normalize(hit.Point - circle.Position);
        return true;
    }
}