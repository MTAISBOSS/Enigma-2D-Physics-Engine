using Physics_Engine.Math;
using System.Linq;
using Physics_Engine.Core.Rigidbody;
using Physics_Engine.Utilities;

namespace Physics_Engine.Core.Collision
{
    public  static class CollisionDetector
    {
        public static bool IntersectAABBs(AABBCollision bodyA,AABBCollision bodyB)
        {
            if (bodyA.Max.x <= bodyB.Min.x || bodyB.Max.x <= bodyA.Min.x)
            {
                return false;
            }
            if (bodyA.Max.y <= bodyB.Min.y || bodyB.Max.y <= bodyA.Min.y)
            {
                return false;
            }
            return true;
        }
        public static bool Intersect(Collider colA, Collider colB, out CollisionInfo collisionInfo)
        {
            collisionInfo = new CollisionInfo();
            if ((colA.Position - colB.Position).Equals(Vector2.Zero))
            {
                collisionInfo.Depth =
                    System.Math.Min(colA.ShapeArea.Calculate(),
                        colB.ShapeArea.Calculate());
                collisionInfo.Normal =
                    new Vector2(RandomHelper.GetRandomFloat(0, 1), RandomHelper.GetRandomFloat(0, 1));
                return true;
            }

            if (colA is CircleCollider circleA && colB is CircleCollider circleB)
            {
                return IntersectCircles(circleA, circleB, out collisionInfo);
            }

            if (colA is CircleCollider circleA1 && colB is PolygonCollider boxB)
            {
                return IntersectCircleWithPolygon(circleA1, boxB,
                    out collisionInfo);
            }

            if (colA is PolygonCollider boxA && colB is CircleCollider circleB1)
            {
                bool result = IntersectCircleWithPolygon(circleB1, boxA,
                    out collisionInfo);
                collisionInfo.Normal = -collisionInfo.Normal;
                return result;
            }

            if (colA is PolygonCollider boxA1 && colB is PolygonCollider boxB1)
            {
                return IntersectPolygons(boxA1, boxB1, out collisionInfo);
            }

            collisionInfo = new CollisionInfo();
            return false;
        }

        #region Intersection Algorithms

        private static bool IntersectCircles(CircleCollider circleA, CircleCollider circleB,
            out CollisionInfo collisionInfo)
        {
            collisionInfo.Depth = 0;
            collisionInfo.Normal = Vector2.Zero;

            float distance = Mathematics.Distance(circleA.Position, circleB.Position);
            float totalRadius = (circleA.CircleArea).Radius +
                                (circleB.CircleArea).Radius;
            if (distance >= totalRadius)
            {
                return false;
            }

            collisionInfo.Normal = Mathematics.Normalize(circleB.Position - circleA.Position);
            collisionInfo.Depth = totalRadius - distance;
            return true;
        }

        private static bool IntersectCircleWithPolygon(CircleCollider circle, PolygonCollider polygon,
            out CollisionInfo collisionInfo)
        {
            collisionInfo.Normal = Vector2.Zero;
            collisionInfo.Depth = float.MaxValue;
            Vector2 axis;
            float minA, maxA, minB, maxB;
            float axisDepth;
            bool isThereGap;
            var vertices = polygon.GetTransformedVertices();
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 vertexA = vertices[i];
                Vector2 vertexB = vertices[i + 1 == vertices.Length ? 0 : i + 1];
                Vector2 edge = vertexB - vertexA;
                axis = new Vector2(-edge.y, edge.x);
                axis = Mathematics.Normalize(axis);

                ProjectVertices(vertices, axis, out minA, out maxA);
                ProjectCircle(circle.Position, circle.CircleArea.Radius, axis, out minB,
                    out maxB);
                isThereGap = minA >= maxB || minB >= maxA;

                if (isThereGap)
                {
                    return false;
                }

                axisDepth = System.Math.Min(maxA - minB, maxB - minA);
                if (axisDepth < collisionInfo.Depth)
                {
                    collisionInfo.Depth = axisDepth;
                    collisionInfo.Normal = axis;
                }
            }

            int closestPointIndex = FindClosestPoint(vertices, circle.Position);
            Vector2 closestPoint = vertices[closestPointIndex];
            axis = closestPoint - circle.Position;
            axis = Mathematics.Normalize(axis);
            ProjectVertices(vertices, axis, out minA, out maxA);
            ProjectCircle(circle.Position, circle.CircleArea.Radius, axis, out minB,
                out maxB);
            isThereGap = minA >= maxB || minB >= maxA;

            if (isThereGap)
            {
                return false;
            }

            axisDepth = System.Math.Min(maxA - minB, maxB - minA);
            if (axisDepth < collisionInfo.Depth)
            {
                collisionInfo.Depth = axisDepth;
                collisionInfo.Normal = axis;
            }

            Vector2 polygonCenter = polygon.Position;

            Vector2 direction = polygonCenter - circle.Position;
            if (Mathematics.DotProduct(direction, collisionInfo.Normal) < 0f)
            {
                collisionInfo.Normal = -collisionInfo.Normal;
            }

            return true;
        }


        private static bool IntersectPolygons(PolygonCollider polygonA, PolygonCollider polygonB,
            out CollisionInfo collisionInfo)
        {
            collisionInfo.Normal = Vector2.Zero;
            collisionInfo.Depth = float.MaxValue;
            var verticesA = polygonA.GetTransformedVertices();
            var verticesB = polygonB.GetTransformedVertices();
            for (int i = 0; i < verticesA.Length; i++)
            {
                Vector2 vertexA = verticesA[i];
                Vector2 vertexB = verticesA[i + 1 == verticesA.Length ? 0 : i + 1];
                Vector2 edge = vertexB - vertexA;
                Vector2 axis = new Vector2(-edge.y, edge.x);
                axis = Mathematics.Normalize(axis);

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                var isThereGap = minA >= maxB || minB >= maxA;

                if (isThereGap)
                {
                    return false;
                }

                float axisDepth = System.Math.Min(maxA - minB, maxB - minA);
                if (axisDepth < collisionInfo.Depth)
                {
                    collisionInfo.Depth = axisDepth;
                    collisionInfo.Normal = axis;
                }
            }

            for (int i = 0; i < verticesB.Length; i++)
            {
                Vector2 vertexA = verticesB[i];
                Vector2 vertexB = verticesB[i + 1 == verticesB.Length ? 0 : i + 1];
                Vector2 edge = vertexB - vertexA;
                Vector2 axis = new Vector2(-edge.y, edge.x);
                axis = Mathematics.Normalize(axis);

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                var isThereGap = minA >= maxB || minB >= maxA;

                if (isThereGap)
                {
                    return false;
                }

                float axisDepth = System.Math.Min(maxA - minB, maxB - minA);
                if (axisDepth < collisionInfo.Depth)
                {
                    collisionInfo.Depth = axisDepth;
                    collisionInfo.Normal = axis;
                }
            }


            Vector2 centerA = polygonA.Position;
            Vector2 centerB = polygonB.Position;

            Vector2 direction = centerB - centerA;
            if (Mathematics.DotProduct(direction, collisionInfo.Normal) < 0f)
            {
                collisionInfo.Normal = -collisionInfo.Normal;
            }

            return true;
        }

        #endregion

        #region Helper Methods

        private static int FindClosestPoint(Vector2[] vertices, Vector2 circleCenter)
        {
            int index = -1;
            float minDistance = float.MaxValue;
            for (int i = 0; i < vertices.Length; i++)
            {
                float dist = Mathematics.Distance(vertices[i], circleCenter);
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
            Vector2 direction = Mathematics.Normalize(axis);
            Vector2 directionAndRadius = direction * radius;

            Vector2 p1 = center + directionAndRadius;
            Vector2 p2 = center - directionAndRadius;

            min = Mathematics.DotProduct(p2, axis);
            max = Mathematics.DotProduct(p1, axis);

            if (max < min)
            {
                (min, max) = (max, min);
            }
        }

        private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
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
        #endregion
    }
}