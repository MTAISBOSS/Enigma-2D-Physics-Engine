using System.Collections.Generic;
using Physics_Engine.Core.Collision;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Raycast;

public static class Raycast2D
{
    public static bool Raycast(Ray2D ray2D, List<Collider> colliders, out RaycastHit2D hitInfo)
    {
        hitInfo = default;
        ray2D.Direction = Mathematics.Normalize(ray2D.Direction);
        float closestDistance = ray2D.Lenght;
        bool hit = false;

        foreach (var collider in colliders)
        {
            if (collider is CircleCollider circle)
            {
                if (RayIntersectCircle(ray2D.Origin, ray2D.Direction, circle, out RaycastHit2D result) &&
                    result.Distance < closestDistance)
                {
                    closestDistance = result.Distance;
                    hitInfo = result;
                    hit = true;
                }
            }
            else if (collider is PolygonCollider polygon)
            {
                if (RayIntersectPolygon(ray2D.Origin, ray2D.Direction, polygon, out RaycastHit2D result) &&
                    result.Distance < closestDistance)
                {
                    closestDistance = result.Distance;
                    hitInfo = result;
                    hit = true;
                }
            }
        }

        return hit;
    }

    private static bool RayIntersectPolygon(Vector2 origin, Vector2 direction, PolygonCollider polygon,
        out RaycastHit2D hit)
    {
        hit = default;

        bool found = false;
        float closestT = float.MaxValue;
        Vector2 bestPoint = Vector2.Zero;
        Vector2 bestNormal = Vector2.Zero;

        var vertices = polygon.TransformedVertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 a = vertices[i];
            Vector2 b = vertices[(i + 1) % vertices.Length];

            if (RayIntersectSegment(origin, direction, a, b, out float t, out Vector2 point))
            {
                if (t >= 0 && t < closestT)
                {
                    closestT = t;
                    bestPoint = point;

                    Vector2 edge = b - a;
                    bestNormal = new Vector2(-edge.y, edge.x);
                    bestNormal = Mathematics.Normalize(bestNormal);

                    found = true;
                }
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

        Vector2 e = b - a;
        float denom = Mathematics.CrossProduct(direction, e);

        if (System.Math.Abs(denom) < 1e-6f)
            return false;

        Vector2 aToOrigin = a - origin;

        t = Mathematics.CrossProduct(aToOrigin, e) / denom;
        float u = Mathematics.CrossProduct(aToOrigin, direction) / denom;

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
        Vector2 oc = origin - circle.Position;
        float radius = circle.CircleArea.Radius;

        float a = Mathematics.LengthSq(direction);
        float b = 2f * Mathematics.DotProduct(oc, direction);
        float c = Mathematics.LengthSq(oc) - (radius * radius);
        //delta = b^2 - 4ac
        //(-b+sqrt(delta)) / 2a
        //(-b-sqrt(delta)) / 2a

        float delta = b * b - 4f * a * c;
        if (delta < 0)
        {
            return false;
        }

        float deltaSqrt = (float)System.Math.Sqrt(delta);
        float t1 = (-b + deltaSqrt) / (2f * a);
        float t2 = (-b - deltaSqrt) / (2f * a);
        float t;
        if (t1 >= 0)
        {
            t = t1;
        }
        else
        {
            if (t2 >= 0)
            {
                t = t2;
            }
            else
            {
                t = -1;
            }
        }


        if (t < 0)
        {
            return false;
        }

        hit.Collider = circle;
        hit.Distance = t;
        hit.Point = origin + direction * t;
        hit.Normal = Mathematics.Normalize(hit.Point - circle.Position);
        return true;
    }
}