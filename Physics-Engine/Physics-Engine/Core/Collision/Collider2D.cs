using System.ComponentModel;
using Physics_Engine.Math;

namespace Physics_Engine.Core.Collision
{
    public static class Collider2D
    {
        public static bool IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB,out Vector2 normal,out float depth)
        {

            normal = Vector2.Zero;
            depth = float.MaxValue;
            
            for (int i = 0; i < verticesA.Length; i++)
            {
                Vector2 vertexA = verticesA[i];
                Vector2 vertexB = verticesA[(i + 1) % verticesA.Length];
                Vector2 edge = vertexB - vertexA;
                Vector2 axis = new Vector2(-edge.y, edge.x);
                
                Project(verticesA, axis, out float minA, out float maxA);
                Project(verticesB, axis, out float minB, out float maxB);

                var isThereGap = minA >= maxB || minB >= maxA;
                
                if (isThereGap)
                {
                    return false;
                }

                float axisDepth = System.Math.Min(maxA - minB, maxB - minA);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }

            }
            for (int i = 0; i < verticesB.Length; i++)
            {
                Vector2 vertexA = verticesB[i];
                Vector2 vertexB = verticesB[(i + 1) % verticesB.Length];
                Vector2 edge = vertexB - vertexA;
                Vector2 axis = new Vector2(-edge.y, edge.x);
                
                Project(verticesA, axis, out float minA, out float maxA);
                Project(verticesB, axis, out float minB, out float maxB);

                var isThereGap = minA >= maxB || minB >= maxA;
                
                if (isThereGap)
                {
                    return false;
                }
                
                float axisDepth = System.Math.Min(maxA - minB, maxB - minA);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            depth /= Mathematics.Length(normal);
            normal = Mathematics.Normalize(normal);
            Vector2 centerA = FindArithmeticMean(verticesA);
            Vector2 centerB = FindArithmeticMean(verticesB);

            Vector2 direction = centerB - centerA;
            if (Mathematics.DotProduct(direction,normal) < 0f)
            {
                normal = -normal;
            }
            return true;
        }

        private static bool HasIntersectPolygons(Vector2[] verticesA, Vector2[] verticesB)
        {
            for (int i = 0; i < verticesA.Length; i++)
            {
                Vector2 vertexA = verticesA[i];
                Vector2 vertexB = verticesA[(i + 1) % verticesA.Length];
                Vector2 edge = vertexB - vertexA;
                Vector2 axis = new Vector2(-edge.y, edge.x);
                
                Project(verticesA, axis, out float minA, out float maxA);
                Project(verticesB, axis, out float minB, out float maxB);

                var isThereGap = minA >= maxB || minB >= maxA;
                
                if (isThereGap)
                {
                    return false;
                }
            }

            return true;
        }

        private static Vector2 FindArithmeticMean(Vector2[] vertices)
        {
            float sumX = 0f;
            float sumY = 0f;

            for (int i = 0; i < vertices.Length; i++)
            {
                sumX += vertices[i].x;
                sumY += vertices[i].y;
            }

            return new Vector2(sumX / vertices.Length, sumY / vertices.Length);
        }
        public static void Project(Vector2[] vertices, Vector2 axis, out float min, out float max)
        {
            max = float.MinValue;
            min = float.MaxValue;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 v = vertices[i];
                float proj = Mathematics.DotProduct(v, axis);
                if (proj < min)
                {
                    min = proj;
                }

                if (proj > max)
                {
                    max = proj;
                }
            }
          
        }
        public static bool Intersect(Vector2 originA, float radiusA, Vector2 originB, float radiusB, out Vector2 normal,
            out float depth)
        {
            depth = 0;
            normal = Vector2.Zero;

            float distance = Mathematics.Distance(originA, originB);
            float totalRadius = radiusA + radiusB;
            if (distance >= totalRadius)
            {
                return false;
            }
            
            normal = Mathematics.Normalize(originB - originA);
            depth = totalRadius - distance;
            return true;
        }
    }
}